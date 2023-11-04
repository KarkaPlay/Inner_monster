using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    [SerializeField] private float hp = 100;
    [SerializeField] private float healPerSec = 1;
    [SerializeField] private float healCooldown = 2;//задержка перед исцелением
    [SerializeField] private float attack = 10;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private float attackCooldown = 1;
    [SerializeField] private float aggressivenessCooldown = 5; // время сколько враг будет злиться(наносить урон) на игрока, обновляется когда враг получет урон
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool EnemyGetHit = false;
    private float aggressivenessTimer = 0;
    private float enemyAttackCooldown = 0;
    public float healCooldownTimer = 0;

    private TextMeshProUGUI HpText;

    private void Start ()
    {
        HpText = GameObject.Find("HP").GetComponent<TextMeshProUGUI>();
    }

    private void Update ()
    {
        //if (!gameObject.CompareTag("Player")) return; // Мы управляем только игроком, потом удалить

        if(gameObject.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.F) && !isAttacking)
                AttackEnemies();

            HpText.SetText("Здоровье: " + (Mathf.Round(hp * 10) / 10).ToString() + "/100");

            Healing();
        }

        if(gameObject.CompareTag("Enemy") && EnemyGetHit)
        {

            if(aggressivenessTimer >= aggressivenessCooldown)
            {
                EnemyGetHit = false;
                aggressivenessTimer = 0;
            }
            else
            {
                AttackPlayer();
                aggressivenessTimer += Time.deltaTime;
            }
        }
    }

    private IEnumerator PlayerAttackCooldown ()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void AttackEnemies ()
    {
        StartCoroutine(PlayerAttackCooldown());

        // Все коллайдеры в радиусе атаки
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        // Если тег Character то наносим урон
        foreach(Collider hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.CompareTag("Enemy"))
            {
                // Должны брать хп из другого скрипта
                Combat enemyCombat = hitCollider.gameObject.GetComponent<Combat>();

                enemyCombat.EnemyTakeDamage(attack);
            }
        }
    }

    public void EnemyTakeDamage (float damage)
    {
        hp -= damage;
        EnemyGetHit = true;
        aggressivenessTimer = 0;
        Debug.Log("ХП врага= " + hp);

        if(hp <= 0)
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

    public void AttackPlayer ()
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
                    // Должны брать хп из другого скрипта
                    Combat player = hitCollider.gameObject.GetComponent<Combat>();

                    player.PlayerTakeDamage(attack);
                }
            }
        }
        else
            enemyAttackCooldown += Time.deltaTime;
    }

    public void PlayerTakeDamage (float damage)
    {
        hp -= damage;
        healCooldownTimer = 0;

        if(hp <= 0)
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
            if(hp < 100)
                hp += healPerSec * Time.deltaTime;
        }
        else
            healCooldownTimer += Time.deltaTime;

    }
}
