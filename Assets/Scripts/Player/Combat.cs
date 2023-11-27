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
    [SerializeField] private float MaxHp = 100;
    [SerializeField] private float MaxSt = 100f;
    [SerializeField] private float healPerSec = 1;
    [SerializeField] private float stRegenPerSec = 1;
    [SerializeField] private float healCooldown = 2;//задержка перед исцелением
    [SerializeField] private float attack = 10;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private float attackCooldown = 1;
    [SerializeField] private float aggressivenessCooldown = 5; // время сколько враг будет злиться(наносить урон) на игрока, обновляется когда враг получет урон
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private GameObject absorbeParticle;

    [SerializeField] private float normalAttackPrice = 5f, strongAttackPrice = 15f, tryAbsorbPrice = 50f, shieldPrice = 5f, powerJumpPrice = 10f;

    private float CurHp;
    private float CurSt;
    private float aggressivenessTimer = 0;
    private float enemyAttackCooldown = 0;
    private GameObject weapon;
    private GameObject shield;
    private bool isAnimActive = false;
    public float healCooldownTimer = 0;
    private bool isShieldActive = false;
    private float shieldTimeout = 0f;
    public Image HpImage;
    public Image StImage;
    public bool EnemyGetHit = false;
    public bool defaultAggressive = false;
    private float[] lastKeyPressTime;
    private KeyCode[] moveKeys = { KeyCode.W, KeyCode.UpArrow, KeyCode.A, KeyCode.LeftArrow, KeyCode.S, KeyCode.DownArrow, KeyCode.D, KeyCode.RightArrow };
    private bool isHidden = true;

    //private TextMeshProUGUI HpText;
    private TextMeshPro EnemyHpText;
    private void Start()
    {
        CurHp = MaxHp;
        CurSt = MaxSt;
       // HpText = GameObject.Find("HP").GetComponent<TextMeshProUGUI>();
        if (gameObject.CompareTag("Enemy"))
        {
            EnemyHpText = transform.Find("Quad").Find("HealthBar").GetComponent<TextMeshPro>();
        }

        weapon = transform.Find("Goat_Armature").Find("Spine").Find("Leg.R").Find("Blade").gameObject;
        if (gameObject.CompareTag("Player"))
        {
            shield = transform.Find("Goat_Armature").Find("Spine").Find("Leg.L").Find("Shield").gameObject;
        }
        shieldTimeout = Time.time;

        lastKeyPressTime = new float[moveKeys.Length];
    }

    IEnumerator ShieldActivate()
    {
        if((shieldTimeout+1f)>Time.time) yield break;
        if (!ReduceStamina(shieldPrice)) yield break;
        shield.transform.localRotation =  Quaternion.Euler(90, -180f, 180);
        isShieldActive = true;
        yield return new WaitForSeconds(.5f);
        shield.transform.localRotation = Quaternion.Euler(90, -90f, 180);
        isShieldActive = false;
    }

    private bool ReduceStamina(float value)
    {
        if ((CurSt - value) >= 0)
        {
            CurSt -= value;
            return true;
        }
        return false;
    }

    private IEnumerator PlayerAttackCooldown ()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private GameObject[] GetNearbyEnemies()
    {
        // Все коллайдеры в радиусе атаки
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        List<GameObject> result = new List<GameObject>();

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Enemy"))
            {
                result.Add(hitCollider.gameObject);
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
       // Debug.Log(Mathf.Abs(angleOne - angleTwo));
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

    public void AggrActivate()
    {
        EnemyGetHit = true;
        aggressivenessTimer = 0;
    }

    private void TryAbsorbEnemy()
    {
        StartCoroutine(PlayerAttackCooldown());
        GameObject[] enemies = GetNearbyEnemies();
        if (enemies.Length == 0) return;
        if (!ReduceStamina(tryAbsorbPrice)) return;
        foreach (GameObject enemy in enemies)
        {
            if (isHidden && !EnemyGetHit && IsLookingAtBack(gameObject, enemy) && IsBehind(gameObject, enemy))
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
        StartCoroutine(PlayerAttackCooldown());
        GameObject[] enemies = GetNearbyEnemies();
        if (enemies.Length == 0) return;
        if (!ReduceStamina(normalAttackPrice)) return;
        SwordAnimation();
        foreach (GameObject enemy in enemies)
        {
            Combat enemyCombat = enemy.GetComponent<Combat>();
            enemyCombat.EnemyTakeDamage(attack);
        }
    }

    public void EnemySetHealthBar(float damage)
    {
        EnemyHpText.SetText((Mathf.Round(CurHp * 10) / 10).ToString() + "/100");
        float hpPercent = CurHp / MaxHp;
        if (hpPercent > 0.66f) EnemyHpText.color = Color.green;
        else if (hpPercent > 0.33f) EnemyHpText.color = Color.yellow;
        else EnemyHpText.color = Color.red;
    }

    private IEnumerator RotateObject(GameObject target)
    {
        isAnimActive = true;
        Quaternion localStartRotation = target.transform.localRotation; // Сохраняем начальную локальную ориентацию
        Quaternion localEndRotation = Quaternion.Euler(55, 0, 0) * localStartRotation; // Вычисляем конечную локальную ориентацию

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

    public void SwordAnimation()
    {
        if (!isAnimActive)
        {
            StartCoroutine(RotateObject(weapon));
        }
    }

    public void EnemyTakeDamage (float damage)
    {
        CurHp -= damage;
        AggrActivate();
        //  Debug.Log("ХП врага= " + CurHp);
        EnemySetHealthBar(damage);
        if (CurHp <= 0)
        {
            Die();
        }
    }

    private void Die ()
    {
        Destroy(gameObject);
    }

    /*private IEnumerator EnemyAttackCooldown ()
    {
        yield return new WaitForSeconds(attackCooldown);
    }*/

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
                if(hitCollider.gameObject.CompareTag("Player"))
                {
                    SwordAnimation();
                    yield return new WaitForSeconds(.3f);
                    Combat player = hitCollider.gameObject.GetComponent<Combat>();
                    if (!player.isShieldActive)
                    {
                        player.PlayerTakeDamage(attack);
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
            //healCooldownTimer = 0;
            if(CurHp < MaxHp)
                CurHp += healPerSec * Time.deltaTime;
        }
        else
            healCooldownTimer += Time.deltaTime;

    }

    private IEnumerator JumpProcess(int direction)
    {
        float jumpDistance = 12f; // Дистанция прыжка
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

        Vector3 jumpVector = jumpRotation * Vector3.forward;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + jumpVector * jumpDistance;

        float startTime = Time.time;

        while (Time.time - startTime < jumpDuration)
        {
            // Используйте параболическую траекторию для учёта высоты
            float normalizedTime = (Time.time - startTime) / jumpDuration;
            float yOffset = jumpHeight * 4f * (normalizedTime - normalizedTime * normalizedTime);
            transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + new Vector3(0, yOffset, 0);

            yield return null;
        }

        // Убедитесь, что объект точно находится на конечной позиции
        transform.position = endPos;
    }

    private void OnDoubleTap(int keyIndex)
    {
        int dir = keyIndex / 2;
        if (!ReduceStamina(powerJumpPrice)) return;
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

        if (gameObject.CompareTag("Player"))
        {
            if (!isAttacking)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    AttackEnemies();
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    TryAbsorbEnemy();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(ShieldActivate());
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

        if (gameObject.CompareTag("Enemy") && EnemyGetHit)
        {

            if (aggressivenessTimer >= aggressivenessCooldown)
            {
                EnemyGetHit = false;
                aggressivenessTimer = 0;
            }
            else
            {
                StartCoroutine(AttackPlayer());
                aggressivenessTimer += Time.deltaTime;
            }
        }
    }
}
