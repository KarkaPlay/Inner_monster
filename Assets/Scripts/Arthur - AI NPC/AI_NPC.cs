using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_NPC : MonoBehaviour
{
    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (AI_NPC_Targets.Instance)
        {
            agent.destination = AI_NPC_Targets.Instance.GetRandomTarget().position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (agent.remainingDistance < 1f)
        {
            agent.destination = AI_NPC_Targets.Instance.GetRandomTarget().position;
        }
    }
}
