using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem System;
    
    private void Start(){
        System = gameObject.GetComponent<ParticleSystem>();
        System.Pause();
    }

    public void Begin(int count){
        System.Emit(count);
    }
}
