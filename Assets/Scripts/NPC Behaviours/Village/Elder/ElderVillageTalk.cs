using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElderVillageTalk : Interactable
{
    [SerializeField]
    private static int trig;
    [SerializeField]
    private NavMeshAgent point;

    public override int TriggerCheck(int trigger){
        switch(trigger){
        case 1:
            trig = 1;
            break;
        case 2:
            QuestCheck(2);
            break;
        case 3:
            trig = 3;
            break;
        case 4:
            trig = 5;
            break;
        }
        return trig;
    }

    public override void QuestCheck(int trigger){
        switch(trigger){
        case 2:
            if (triggercolis.IsEntered){
                trig = 2;
            } else {
                trig = 1;
            }
            break;
        }
    }

    public void FixedUpdate(){
        switch(Trigger){
        case 1:
            point.destination = new Vector3(-66, 2, 92);
            break;
        case 3:
            point.destination = new Vector3(-111, 0, 80);
            break;
        case 4:
            point.destination = new Vector3(-106, 2, 166);
            break;
        }    
    }
}