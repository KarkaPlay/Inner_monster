using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AI_NPC_Targets : MonoBehaviour
{
    public static List<Transform> targets;
    public static Transform randomTarget;

    private static int randomInd;
    
    public static AI_NPC_Targets Instance;

    private void Awake()
    {
        GetTargetsFromParent();
        Instance = this;  
    }
    
    private void GetTargetsFromParent()
    {
        targets = new List<Transform>();
        foreach (Transform child in gameObject.transform)
        {
            targets.Add(child.gameObject.transform);
        }
    }
    
    public Transform GetRandomTarget()
    {
        randomInd = Random.Range(0, targets.Count);
        randomTarget = targets[randomInd];
        return randomTarget;
    }
}
