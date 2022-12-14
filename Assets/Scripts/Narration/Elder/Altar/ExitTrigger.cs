using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public static bool isEntered;
    
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player"){
            isEntered = true;
        }
    }
}
