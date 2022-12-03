using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float smooth = 0.1f;//скорость сопровождения
    public GameObject target;//объект сопровождения

    void FixedUpdate()
    {
        Vector3 pos = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, pos, smooth);
    }
}
