using UnityEngine;
using UnityEngine.AI; // Добавляем пространство имен для работы с NavMeshAgent

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    public float minDistance = 2f;
    public float visionAngle = 45f;
    public float visionRange = 20f;
    public float aggrDeactivateTreshold = 3; // время через которое нпс отагриться от игрока, если он вне поля зрения

    private NavMeshAgent agent;
    private Combat combatObj;
    private Vector3 startPos;
    private Quaternion startRot;
    private float timeSinceLastSeen = 0f; // Таймер для отслеживания времени, прошедшего с момента последнего "видения" игрока
    private bool playerInSightLastFrame = false; // Был ли игрок в поле зрения в последнем кадре
    private float moveRandomTimer = 0f;

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

        //print((int)timeSinceLastSeen);
        // Обновление таймера, если игрок вышел из поля зрения
        if (playerInSightLastFrame && !canSeePlayerNow)
        {
            timeSinceLastSeen += Time.deltaTime;
            MoveRandomly();
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
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float bufferZone = 1f; // Буферная зона для предотвращения колебаний


        Vector3 playerForward = player.forward;
        float angleBetweenPlayerAndNPC = Vector3.Angle(-playerForward, directionToPlayer);
        if (angleBetweenPlayerAndNPC > 90f) // Игрок стоит спиной к NPC
        {
            distanceToPlayer += 1.5f;
        }

        // Проверяем, находится ли NPC внутри буферной зоны
        if (distanceToPlayer > minDistance + bufferZone)
        {
            // Если игрок дальше минимальной дистанции плюс буфер, преследуем его
            agent.SetDestination(player.position);
        }
        else if (distanceToPlayer < minDistance - bufferZone)
        {
            // Если игрок слишком близко, отступаем назад
            Vector3 directionFromPlayer = (transform.position - player.position).normalized;
            Vector3 retreatPosition = transform.position + directionFromPlayer * bufferZone; // Расчет позиции для отступления
            agent.SetDestination(retreatPosition);
        }
        else
        {
            // Останавливаем NPC, если он находится внутри буферной зоны
            agent.ResetPath();
        }
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
    }

    void MoveRandomly()
    {
        float MoveSetTime = 0.3f; // Время между изменениями направления
        float MoveRadius = 3f;

        moveRandomTimer += Time.deltaTime;
        if (moveRandomTimer >= MoveSetTime)
        {
            // Генерируем случайную точку в заданном радиусе
            Vector3 randomDirection = Random.insideUnitSphere * MoveRadius;
            randomDirection += transform.position; // Добавляем текущую позицию, чтобы сместить точку

            // Находим ближайшую доступную точку на NavMesh
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, MoveRadius, -1);

            // Задаем найденную точку как новую цель для NavMeshAgent
            agent.SetDestination(hit.position);

            // Сбрасываем таймер
            moveRandomTimer = 0f;
        }
    }
}