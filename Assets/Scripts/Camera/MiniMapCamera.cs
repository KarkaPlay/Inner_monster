using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public GameObject player;

    public float distance = 30;
    void Start ()
    {
        player = GameObject.FindWithTag("Player");
    }

    void LateUpdate ()
    {
        transform.position = player.transform.position + Vector3.up * distance;
    }
}
