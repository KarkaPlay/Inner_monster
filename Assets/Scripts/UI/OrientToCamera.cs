using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientToCamera : MonoBehaviour
{
    private Transform mainCam;
    
    void Awake(){
        mainCam = Camera.main.transform;
    }
    
    void LateUpdate(){
        transform.LookAt(mainCam);
    }
}
