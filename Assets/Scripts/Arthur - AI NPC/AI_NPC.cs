using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_NPC : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;
    public GameObject target4;

    private List<GameObject> targets = new List<GameObject>();
    private GameObject randomTarget;
    private int randomInd;
    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        targets.Add(target1);
        targets.Add(target2);
        targets.Add(target3);
        targets.Add(target4);
        
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!agent.hasPath)
        {
            randomInd = Random.Range(0, targets.Count);
            randomTarget = targets[randomInd];
            agent.destination = randomTarget.transform.position;
        }
    }
    
}
