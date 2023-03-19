using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistRikeTalk : Interactable
{
    [SerializeField]
    private static int trig;

    public override int TriggerCheck(int trigger){
        return 0;
    }

    public override void QuestCheck(int Trigger){
        
    }
}
