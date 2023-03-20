using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat2 : MonoBehaviour
{
    public float hp = 100;
    public float attack = 10;
    public bool isAttacking = false;
    public float attackCooldown = 1;
    public float attackRange = 3;

    public void Attack()
    {
        StartCoroutine(AttackCooldown());
        
        // Получаем список всех врагов
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Character");
        Debug.Log("Enemies: " + enemies.Length);
    
        // Если врагов нет, то выходим из функции
        if (enemies.Length == 0)
        {
            Debug.Log("No enemies");
            return;
        }

        // Находим ближайшего врага
        GameObject closestEnemy = enemies[0];
        Debug.Log("Closest enemy: " + closestEnemy.name);
        float closestDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);
        Debug.Log("Closest distance: " + closestDistance);
        for (int i = 1; i < enemies.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, enemies[i].transform.position);
            if (distance < closestDistance)
            {
                Debug.Log("New closest enemy: " + enemies[i].name);
                closestDistance = distance;
                closestEnemy = enemies[i];
            }
        }
        
        // Атакуем ближайшего врага если он в пределах атаки
        if (closestDistance <= attackRange)
        {
            Debug.Log("Attacking " + closestEnemy.name);
            // Поворачиваемся к врагу
            StartCoroutine(LookAtEnemy(closestEnemy));
            closestEnemy.GetComponent<Combat2>().TakeDamage(attack);
        }
    }
    
    // Персонаж получает урон
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
            Die();
    }
    
    // Персонаж умирает
    public void Die()
    {
        Destroy(gameObject);
    }
    
    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
    
    // Смотрим на врага attackCooldown секунд
    IEnumerator LookAtEnemy(GameObject enemy)
    {
        transform.LookAt(enemy.transform);
        yield return new WaitForSeconds(attackCooldown);
        transform.LookAt(Vector3.zero);
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
}
