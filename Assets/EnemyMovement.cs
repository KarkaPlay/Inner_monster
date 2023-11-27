using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float minDistance = 2f;

    public float visionAngle = 45f;  // Угол зрения врага
    public float visionRange = 20f;  // Дальность зрения врага

    private Rigidbody rb;
    private Combat combatObj;
    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        combatObj = GetComponent<Combat>();
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void FixedUpdate()
    {
        if (combatObj.EnemyGetHit)
        {
            HauntPlayer();
        }
        else if (combatObj.defaultAggressive && CanSeePlayer())
        {
            combatObj.AggrActivate();  // Вызываем функцию AggrActivate() при наличии игрока в поле зрения
        }
        else if (Vector2.Distance(new Vector2(startPos.x, startPos.z), new Vector2(transform.position.x, transform.position.z)) > 0.1f)
        {
            ReturnToHomePos();
        }
        else if (transform.rotation != startRot)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, startRot, Time.deltaTime * 5f);
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        return angleToPlayer <= visionAngle && distanceToPlayer <= visionRange;
    }


    void ReturnToHomePos()
    {
        Vector3 directionToHome = (startPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToHome);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        float distanceToHome = Vector3.Distance(transform.position, startPos);
        Vector3 newPosition = transform.position + directionToHome * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }


    void HauntPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= minDistance) return;
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }
}