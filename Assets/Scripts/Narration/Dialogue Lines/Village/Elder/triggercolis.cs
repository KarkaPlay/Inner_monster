using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggercolis : MonoBehaviour
{
    public static bool IsEntered;

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player"){
            IsEntered = true;
        }
    }
}
