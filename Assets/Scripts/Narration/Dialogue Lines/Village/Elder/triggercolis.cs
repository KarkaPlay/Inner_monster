using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static trig;
public class triggercolis : MonoBehaviour
{
    public GameObject trigger;
    private int k=0;
    void OnTriggerEnter(Collider other)
    {
      
        if (other.tag == "Player"&& (k>=1))
        {
            trig.contact = true;
            trigger.SetActive(false);
            trig.n++;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            k++;
        }
    }
}
