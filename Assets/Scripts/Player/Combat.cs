using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    [Header("Main stats")]
    [SerializeField] private float MaxHp = 100;
    [SerializeField] private float MaxSt = 100f; // стамина ака выносливость
    [SerializeField] private float attackValue = 10;
    [SerializeField] private float attackRange = 1;
    public bool defaultAggressive = false;
    public bool isHidden = true; // влияет на возможность схавать нпс

    [Header("Cooldowns")]
    [SerializeField] private float healPerSec = 1;
    [SerializeField] private float stRegenPerSec = 1;
    [SerializeField] private float healCooldown = 2;//задержка перед исцелением
    [SerializeField] private float attackCooldown = 1;
    [SerializeField] private float aggressivenessCooldown = 999; // время сколько враг будет злиться(наносить урон) на игрока, обновляется когда враг получет урон
    [SerializeField] private float aggrMinDuration = 3; // минимальное время, через которое нпс может потерять игрока из вида и сбросить агр

    [Header("Stamina price")]
    [SerializeField] private float normalAttackPrice = 5f;
    [SerializeField] private float strongAttackPrice = 15f;
    [SerializeField] private float tryAbsorbPrice = 50f;
    [SerializeField] private float shieldPrice = 5f;
    [SerializeField] private float powerJumpPrice = 10f;
    public float runSecondPrice = 5f;
    public float jumpPrice = 5f;

    [Header("Other")]
    [SerializeField] private GameObject absorbeParticle;
    [SerializeField] private Image HpImage;
    [SerializeField] private Image StImage;


    [HideInInspector] public bool IsAgressive = false;
    private float CurHp;
    private float CurSt;
    private float aggressivenessTimer = 0;
    private float enemyAttackCooldown = 0;
    private GameObject weapon;
    private GameObject shield;
    private bool isAnimActive = false;
    private bool isShieldActive = false;
    private float shieldTimeout = 0f;
    private float[] lastKeyPressTime;
    private float healCooldownTimer = 0;
    private KeyCode[] moveKeys = { KeyCode.W, KeyCode.UpArrow, KeyCode.A, KeyCode.LeftArrow, KeyCode.S, KeyCode.DownArrow, KeyCode.D, KeyCode.RightArrow };
    private int lastAttackType = 0, attackTypesCount = 3; // для серии атак
    private float lastAttackTime = 0, attackSeriesTheshold = 3f;
    private float holdTime = 0f;
    private float powerAttackThreshold = 1f;
    private bool isPowerAttack = false;
    private bool isAttacking = false;
    private TextMeshPro EnemyHpText;
    private StarterAssets.ThirdPersonController playerController;
    private float lastDashTime = 0f;
    private bool isDashActive = false;
    private MainMenuActivator mainMenuAct;
    private bool isPlayer;
    private ObjectsInteraction objInt;

    private void Start()
    {
        CurHp = MaxHp;
        CurSt = MaxSt;
        isPlayer = gameObject.CompareTag("Player") ? true : false;

        if (!isPlayer)
        {
            EnemyHpText = transform.Find("Quad").Find("HealthBar").GetComponent<TextMeshPro>();
        }

        weapon = transform.Find("Goat_Armature").Find("Spine").Find("Leg.R").Find("Blade").gameObject;
        if (isPlayer)
        {
            shield = transform.Find("Goat_Armature").Find("Spine").Find("Leg.L").Find("Shield").gameObject;
        }

        shieldTimeout = Time.time;
        lastAttackTime = Time.time - attackSeriesTheshold;

        lastKeyPressTime = new float[moveKeys.Length];
        playerController = GetComponent<StarterAssets.ThirdPersonController>();
        mainMenuAct = FindObjectOfType<MainMenuActivator>();
        objInt = GetComponent<ObjectsInteraction>();
    }

    IEnumerator ShieldActivate()
    {
        shield.transform.localRotation = Quaternion.Euler(90, -180f, 180);
        isShieldActive = true;
        // Цикл работает пока игрок удерживает ПКМ
        while (Input.GetMouseButton(1))
        {
            if (!ReduceStamina(shieldPrice * Time.deltaTime)) // Постепенное уменьшение стамины
            {
                StartCoroutine(ShieldDeactivate());
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator ShieldDeactivate()
    {
        shield.transform.localRotation = Quaternion.Euler(90, -90f, 180);
        isShieldActive = false;
        yield return null; // Это можно опустить, если дальнейшие действия не требуют задержки
    }

    public bool ReduceStamina(float value)
    {
        if ((CurSt - value) >= 0)
        {
            CurSt -= value;
            return true;
        }
        return false;
    }

    private IEnumerator PlayerAttackCooldown (float bonusTime=0f)
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackCooldown + bonusTime);
        isAttacking = false;
    }

    private GameObject[] GetNearbyEnemies()
    {
        // Все коллайдеры в радиусе атаки
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        List<GameObject> result = new List<GameObject>();

        // Желаемый угол обзора
        float fieldOfViewDegrees = 90f; // Угол обзора 90 градусов

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Enemy"))
            {
                Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
                // Угол между направлением взгляда игрока и направлением на цель
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                // Проверяем, находится ли враг в поле зрения
                if (angleToTarget < fieldOfViewDegrees / 2)
                {
                    result.Add(hitCollider.gameObject);
                }
            }
        }
        return result.ToArray();
    }

    private float GetForwardAngle(GameObject target)
    {
        Vector3 forwardDirection = target.transform.forward;
        forwardDirection.y = 0;
        float angleRad = Mathf.Atan2(forwardDirection.z, forwardDirection.x);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        /*if (angleDeg < 0)
        {
            angleDeg += 360f;
        }*/
        return angleDeg;
    }

    private bool IsLookingAtBack(GameObject observer, GameObject target)
    {
        float angleOne = GetForwardAngle(observer);
        float angleTwo = GetForwardAngle(target);
        if (Mathf.Abs(angleOne - angleTwo)<60f)
        {
            return true;
        }
        return false;
    }

    bool IsBehind(GameObject obj2, GameObject obj1)
    {
        float backDistance = 2f;
        float detectionDistance = 5f;

        Vector3 position1 = obj1.transform.position;
        Vector3 position2 = obj2.transform.position;

        Vector3 relativePosition = position2 - position1;
        Vector3 backPosition = position1 - obj1.transform.forward * backDistance;

        // Проверка, находится ли obj2 за спиной obj1
        return Vector3.Dot(relativePosition, obj1.transform.forward) < 0 && Vector3.Distance(position2, backPosition) < detectionDistance;
    }

    private IEnumerator DestroyObj(GameObject target, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(target);
    }

    private void SetAggr(bool state)
    {
        IsAgressive = state;
        aggressivenessTimer = 0;
    }

    public void AggrActivate()
    {
        SetAggr(true);
    }

    public void AggrDeactivate()
    {
        if (aggressivenessTimer < aggrMinDuration) return; // трешхолд минималка агрессии
        SetAggr(false);
    }

    private void TryAbsorbEnemy()
    {
        StartCoroutine(PlayerAttackCooldown());
        GameObject[] enemies = GetNearbyEnemies();
        if (enemies.Length == 0) return;
        if (!ReduceStamina(tryAbsorbPrice)) return;
        foreach (GameObject enemy in enemies)
        {
            if (isHidden && !IsAgressive && IsLookingAtBack(gameObject, enemy) && IsBehind(gameObject, enemy))
            {
                GameObject particle = GameObject.Instantiate(absorbeParticle,
                enemy.transform.position,
                Quaternion.identity) as GameObject;
                StartCoroutine(DestroyObj(particle, 1.5f));
                Destroy(enemy);
            }
            else
            {
                Combat enemyCombat = enemy.GetComponent<Combat>();
                enemyCombat.AggrActivate();
            }
        }
    }

    private void AttackEnemies ()
    {
        StartCoroutine(PlayerAttackCooldown(isPowerAttack?1f:0f));
        if (!ReduceStamina(isPowerAttack ? strongAttackPrice : normalAttackPrice)) return;
        SwordAnimation(lastAttackType);
        GameObject[] enemies = GetNearbyEnemies();
        if (enemies.Length == 0) return;

        float curDamage = attackValue * (isPowerAttack ? 2 : 1);
        if (lastAttackTime + attackSeriesTheshold > Time.time)
        {
            lastAttackType = (lastAttackType + 1) % attackTypesCount;
        }
        else { lastAttackType = 0; }

        lastAttackTime = Time.time;

        foreach (GameObject enemy in enemies)
        {
            Combat enemyCombat = enemy.GetComponent<Combat>();
            enemyCombat.EnemyTakeDamage(curDamage);
        }
    }

    public void EnemySetHealthBar()
    {
        EnemyHpText.SetText((Mathf.Round(CurHp * 10) / 10).ToString() + "/100");
        float hpPercent = CurHp / MaxHp;
        if (hpPercent > 0.66f) EnemyHpText.color = Color.green;
        else if (hpPercent > 0.33f) EnemyHpText.color = Color.yellow;
        else EnemyHpText.color = Color.red;
    }

    private IEnumerator RotateObject(GameObject target, int attackType = 0)
    {
        isAnimActive = true;
        Vector3 newOffset;
        switch (attackType)
        {
            case 0:
                newOffset = new Vector3(55, 0, 0);
                break;
            case 1:
                newOffset = new Vector3(0, 40, 0);
                break;
            case 2:
                newOffset = new Vector3(80, 80, 0);
                break;
            default:
                yield break;
        }
        Quaternion localStartRotation = target.transform.localRotation; // Сохраняем начальную локальную ориентацию
        Quaternion localEndRotation = Quaternion.Euler(newOffset) * localStartRotation; // Вычисляем конечную локальную ориентацию

        float duration = 0.3f;
        float time = 0;

        while (time < duration)
        {
            target.transform.localRotation = Quaternion.Lerp(localStartRotation, localEndRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        target.transform.localRotation = localEndRotation;
        yield return new WaitForSeconds(.1f);

        time = 0;
        while (time < duration)
        {
            target.transform.localRotation = Quaternion.Lerp(localEndRotation, localStartRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        target.transform.localRotation = localStartRotation;
        isAnimActive = false;
    }

    IEnumerator ScaleAndMove(GameObject obj, float scaleFactor, Vector3 offset, float duration)
    {
        Vector3 originalScale = obj.transform.localScale;
        Vector3 targetScale = originalScale * scaleFactor;

        Vector3 originalPosition = obj.transform.localPosition;
        Vector3 targetPosition = originalPosition + offset;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            obj.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            obj.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsed / duration);

            elapsed += Time.deltaTime;

            yield return null;
        }

        obj.transform.localScale = targetScale;
        obj.transform.localPosition = targetPosition;
    }

    IEnumerator PowerAttack(GameObject obj, float scaleFactor, Vector3 offset, float duration)
    {
        yield return ScaleAndMove(obj ,scaleFactor, offset, duration);
        yield return StartCoroutine(RotateObject(weapon, 2));
        yield return ScaleAndMove(obj, 1/scaleFactor, offset, duration);
    }

    public void SwordAnimation(int attackType = 0)
    {
        if (!isAnimActive)
        {
            if (!isPowerAttack)
            {
                StartCoroutine(RotateObject(weapon, attackType));
            }
            else
            {
                StartCoroutine(PowerAttack(weapon, 2, new Vector3(0, 0, 0), 0.2f));
                isPowerAttack = false;
            }
        }
    }

    public void EnemyTakeDamage (float damage)
    {
        CurHp -= damage;
        AggrActivate();
        EnemySetHealthBar();
        if (CurHp <= 0)
        {
            Die();
        }
    }

    private void Die ()
    {
        Destroy(gameObject);
    }

    public IEnumerator AttackPlayer ()
    {
        //StartCoroutine(EnemyAttackCooldown());
        if(enemyAttackCooldown >= attackCooldown)
        {
            enemyAttackCooldown = 0;

            // Все коллайдеры в радиусе атаки
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);


            // Если тег Character то наносим урон
            foreach(Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Player"))
                {
                    SwordAnimation();
                    yield return new WaitForSeconds(.3f);
                    Combat player = hitCollider.gameObject.GetComponent<Combat>();
                    // Направление от игрока к NPC
                    Vector3 directionToNpc = (transform.position - player.transform.position).normalized;
                    // Направление взгляда игрока
                    Vector3 playerDirection = player.transform.forward;

                    // Рассчитываем угол между направлением взгляда игрока и направлением к NPC
                    float angle = Vector3.Angle(playerDirection, directionToNpc);

                    if ((!player.isShieldActive || angle > 40) && !isDashActive && (lastDashTime + 0.5f < Time.time))
                    {
                        player.PlayerTakeDamage(attackValue);
                    }
                }
            }
        }
        else
            enemyAttackCooldown += Time.deltaTime;
    }

    public void PlayerTakeDamage (float damage)
    {
        CurHp -= damage;
        healCooldownTimer = 0;

        if(CurHp <= 0)
        {
            //Die();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Healing ()
    {
        if(healCooldownTimer >= healCooldown)
        {
            if(CurHp < MaxHp)
                CurHp += healPerSec * Time.deltaTime;
        }
        else
            healCooldownTimer += Time.deltaTime;

    }

    private IEnumerator JumpProcess(int direction)
    {
        isDashActive = true;
        CharacterController controller = GetComponent<CharacterController>();

        float jumpDistance = 6f; // Дистанция прыжка
        float jumpDuration = 0.1f; // Продолжительность прыжка
        float jumpHeight = 0.5f; // Высота прыжка

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; // Игнорируем вертикальную составляющую вектора камеры

        Quaternion jumpRotation = Quaternion.LookRotation(cameraForward);

        switch (direction)
        {
            case 0: // вперёд
                break;
            case 1: // лево
                jumpRotation *= Quaternion.Euler(0, -90, 0);
                break;
            case 2: // назад
                jumpRotation *= Quaternion.Euler(0, 180, 0);
                break;
            case 3: // право
                jumpRotation *= Quaternion.Euler(0, 90, 0);
                break;
            default:
                break;
        }

        Vector3 jumpVector = jumpRotation * Vector3.forward * jumpDistance;

        float startTime = Time.time;

        while (Time.time - startTime < jumpDuration)
        {
            float normalizedTime = (Time.time - startTime) / jumpDuration;
            float yOffset = jumpHeight * 4f * (normalizedTime - normalizedTime * normalizedTime);

            // Вычисляем вектор перемещения для текущего кадра
            Vector3 frameMove = Vector3.Lerp(Vector3.zero, jumpVector, normalizedTime) - Vector3.Lerp(Vector3.zero, jumpVector, normalizedTime - Time.deltaTime / jumpDuration);
            frameMove.y = yOffset - (jumpHeight * 4f * ((normalizedTime - Time.deltaTime / jumpDuration) - (normalizedTime - Time.deltaTime / jumpDuration) * (normalizedTime - Time.deltaTime / jumpDuration)));
            controller.Move(frameMove);

            yield return null;
        }
        lastDashTime = Time.time;
        isDashActive = false;
    }

    private void OnDoubleTap(int keyIndex)
    {
        int dir = keyIndex / 2;
        if (!(playerController._verticalVelocity <= 0) || !ReduceStamina(powerJumpPrice)) return;
        StartCoroutine(JumpProcess(dir));
    }

    private void DoubleTapCheck()
    {
        float doubleTapThreshold = .3f; 

        for (int i = 0; i < moveKeys.Length; i++)
        {
            KeyCode key = moveKeys[i];
            if (Input.GetKeyDown(key))
            {

                if (((Time.time - lastKeyPressTime[i]) < doubleTapThreshold) && lastKeyPressTime[i]!=0)
                {
                    lastKeyPressTime[i] = 0;
                    OnDoubleTap(i);
                }
                else
                {
                    lastKeyPressTime[i] = Time.time;
                }
            }

        }
    }

    private void Update()
    {
        
        if (isPlayer)
        {
            if (mainMenuAct.EscapeMenuOpen || playerController.isMapOpen) return;
            if (objInt.isHoldingObject) return;

            if (Input.GetMouseButtonDown(0))
            {
                holdTime = Time.time;
            }
            else if (Input.GetMouseButton(0))
            {
                if (Time.time - holdTime > powerAttackThreshold)
                {
                    isPowerAttack = true;
                }
            }
            else if (Input.GetMouseButtonUp(0)&& !isAttacking)
            {
                AttackEnemies();
                holdTime = Time.time + (powerAttackThreshold*5);
            }
            else if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
            {
                TryAbsorbEnemy();
            }

            // Проверяем, начало удерживания ПКМ
            if (Input.GetMouseButtonDown(1))
            {
                if (lastDashTime + 1f < Time.time)
                {
                    // Начинаем активацию щита
                    if (!isShieldActive && ReduceStamina(shieldPrice))
                    {
                        StartCoroutine(ShieldActivate());
                    }
                }
            }
            // Проверяем, окончание удерживания ПКМ
            else if (Input.GetMouseButtonUp(1))
            {
                // Останавливаем активацию щита
                StopCoroutine(ShieldActivate());
                StartCoroutine(ShieldDeactivate());
            }

            DoubleTapCheck();

            HpImage.fillAmount = 0.2f + (CurHp / MaxHp) * 0.8f;
            StImage.fillAmount = ((Mathf.Round(CurSt * 10) / 10) / MaxSt) * 0.8f;
            if (CurSt < MaxSt)
            {
                CurSt += (stRegenPerSec * Time.deltaTime);
            }
            Healing();
        }

        if (!isPlayer && IsAgressive)
        {

            if (aggressivenessTimer >= aggressivenessCooldown)
            {
                AggrDeactivate();
            }
            else
            {
                StartCoroutine(AttackPlayer());
                aggressivenessTimer += Time.deltaTime;
            }
        }
    }
}
