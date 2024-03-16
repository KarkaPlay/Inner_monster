using UnityEngine;
using UnityEngine.AI; // ��������� ������������ ���� ��� ������ � NavMeshAgent

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    public float minDistance = 2f;
    public float visionAngle = 45f;
    public float visionRange = 20f;
    public float aggrDeactivateTreshold = 3; // ����� ����� ������� ��� ���������� �� ������, ���� �� ��� ���� ������

    private NavMeshAgent agent;
    private Combat combatObj;
    private Vector3 startPos;
    private Quaternion startRot;
    private float timeSinceLastSeen = 0f; // ������ ��� ������������ �������, ���������� � ������� ���������� "�������" ������
    private bool playerInSightLastFrame = false; // ��� �� ����� � ���� ������ � ��������� �����
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
            timeSinceLastSeen = 0f; // ����� �������, ��� ��� ����� � ���� ������
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
        // ���������� �������, ���� ����� ����� �� ���� ������
        if (playerInSightLastFrame && !canSeePlayerNow)
        {
            timeSinceLastSeen += Time.deltaTime;
            MoveRandomly();
        }
        else if (canSeePlayerNow)
        {
            timeSinceLastSeen = 0f; // ����� �������, ���� ����� ����� �������� � ���� ������
            playerInSightLastFrame = true; // ���������� ��������� ��������� ������
        }
        else
        {
            playerInSightLastFrame = false; // ����� �� � ���� ������
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
        float bufferZone = 1f; // �������� ���� ��� �������������� ���������


        Vector3 playerForward = player.forward;
        float angleBetweenPlayerAndNPC = Vector3.Angle(-playerForward, directionToPlayer);
        if (angleBetweenPlayerAndNPC > 90f) // ����� ����� ������ � NPC
        {
            distanceToPlayer += 1.5f;
        }

        // ���������, ��������� �� NPC ������ �������� ����
        if (distanceToPlayer > minDistance + bufferZone)
        {
            // ���� ����� ������ ����������� ��������� ���� �����, ���������� ���
            agent.SetDestination(player.position);
        }
        else if (distanceToPlayer < minDistance - bufferZone)
        {
            // ���� ����� ������� ������, ��������� �����
            Vector3 directionFromPlayer = (transform.position - player.position).normalized;
            Vector3 retreatPosition = transform.position + directionFromPlayer * bufferZone; // ������ ������� ��� �����������
            agent.SetDestination(retreatPosition);
        }
        else
        {
            // ������������� NPC, ���� �� ��������� ������ �������� ����
            agent.ResetPath();
        }
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
    }

    void MoveRandomly()
    {
        float MoveSetTime = 0.3f; // ����� ����� ����������� �����������
        float MoveRadius = 3f;

        moveRandomTimer += Time.deltaTime;
        if (moveRandomTimer >= MoveSetTime)
        {
            // ���������� ��������� ����� � �������� �������
            Vector3 randomDirection = Random.insideUnitSphere * MoveRadius;
            randomDirection += transform.position; // ��������� ������� �������, ����� �������� �����

            // ������� ��������� ��������� ����� �� NavMesh
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, MoveRadius, -1);

            // ������ ��������� ����� ��� ����� ���� ��� NavMeshAgent
            agent.SetDestination(hit.position);

            // ���������� ������
            moveRandomTimer = 0f;
        }
    }
}