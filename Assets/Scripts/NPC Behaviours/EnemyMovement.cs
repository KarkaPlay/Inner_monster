using UnityEngine;
using UnityEngine.AI; // Добавляем пространство имен для работы с NavMeshAgent

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    public float minDistance = 2f;
    public float visionAngle = 45f;
    public float visionRange = 20f;
    public float aggrDeactivateTreshold = 3f; // время через которое нпс отагриться от игрока, если он вне поля зрения

    private NavMeshAgent agent;
    private Combat combatObj;
    private Vector3 startPos;
    private Quaternion startRot;
    private float timeSinceLastSeen = 0f; // Таймер для отслеживания времени, прошедшего с момента последнего "видения" игрока
    private bool playerInSightLastFrame = false; // Был ли игрок в поле зрения в последнем кадре

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        combatObj = GetComponent<Combat>();
        startPos = transform.position;
        startRot = transform.rotation;
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    void Update()
    {

        if (combatObj.IsAgressive)
        {
            HauntPlayer();
        }
        else if (combatObj.defaultAggressive && CanSeePlayer())
        {
            combatObj.AggrActivate();
            HauntPlayer();
            timeSinceLastSeen = 0f; // Сброс таймера, так как игрок в поле зрения
        }
        else if (!NearStartPosition())
        {
            ReturnToHomePos();
        }
        else
        {
            ResetToStartPosition();
        }

        float distance = Vector3.Distance(transform.position, player.position);
        bool canSeePlayerNow = (CanSeePlayer() || (distance < 15));

        // Обновление таймера, если игрок вышел из поля зрения
        if (playerInSightLastFrame && !canSeePlayerNow)
        {
            timeSinceLastSeen += Time.deltaTime;
        }
        else if (canSeePlayerNow)
        {
            timeSinceLastSeen = 0f; // Сброс таймера, если игрок снова попадает в поле зрения
            playerInSightLastFrame = true; // Обновление состояния видимости игрока
        }
        else
        {
            playerInSightLastFrame = false; // Игрок не в поле зрения
        }

        if (timeSinceLastSeen >= aggrDeactivateTreshold && combatObj.IsAgressive)
        {
            combatObj.AggrDeactivate();
            timeSinceLastSeen = 0f;
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
        agent.SetDestination(startPos);
    }

    void ResetToStartPosition()
    {
        if (transform.rotation != startRot)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, startRot, Time.deltaTime * 5f);
        }
    }

    bool NearStartPosition()
    {
        float distanceToStartPos = Vector3.Distance(transform.position, startPos);
        return distanceToStartPos < 0.1f;
    }

    void HauntPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > minDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
        }
    }
}