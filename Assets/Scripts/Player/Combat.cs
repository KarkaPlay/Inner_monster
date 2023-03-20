using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private float hp = 100;
    [SerializeField] private float attack = 10;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private float attackCooldown = 1;
    [SerializeField] private bool isAttacking = false;

    private void Update()
    {
        if (!gameObject.CompareTag("Player")) return; // Мы управляем только игроком, потом удалить
        
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            Debug.Log("Attack");
            Attack();
        }
    }

    private IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void Attack()
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

    private void Die()
    {
        Destroy(gameObject);
    }
}
