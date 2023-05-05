using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonologueTime : MonoBehaviour
{
    public static int time;

    public void Awake(){
        time = 0;
    }

    public void FixedUpdate(){
        time++; 
    }
}