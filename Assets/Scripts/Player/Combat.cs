using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public float hp = 100;
    public float attack = 10;
    public float attackRange = 1;
    public float attackCooldown = 1;
    public bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.CompareTag("Player")) return; // Мы управляем только игроком, потом удалить
        
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            Debug.Log("Attack");
            Attack();
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
    
    public void Attack()
    {
        StartCoroutine(AttackCooldown());
        
        // Все коллайдеры в радиусе атаки
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        
        // Если тег Character то наносим урон
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Character"))
            {
                // Должны брать хп из другого скрипта
                Combat enemyCombat = hitCollider.gameObject.GetComponent<Combat>();
                
                enemyCombat.TakeDamage(attack);
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        hp -= damage;
        
        if (hp <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }
}
