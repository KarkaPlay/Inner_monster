using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalIsNear : MonoBehaviour
{
    private Transformation transformationScript;
    ParticleController ParticleControllerScript;
    private bool newTransform = false;
    private bool readyToTransformBack = false;
    private bool alreadyTransformed = false;
    private bool readyToTransform = false;

    [SerializeField]private int countOfParticles;

    private void Start()
    {
        ParticleControllerScript = GetComponent<ParticleController>();
        transformationScript = GetComponent<Transformation>();
    }

    private void Update(){
        if (Input.GetButtonDown("TransformationBack")){
            if (readyToTransformBack){
                ParticleControllerScript.Begin(countOfParticles);
                transformationScript.Transform_back();  
                readyToTransformBack = false;
                readyToTransform = true;
                alreadyTransformed = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Animal")){
            readyToTransform = true;
        }
    }
    
    private void OnTriggerStay(Collider other){
        if (other.CompareTag("Animal")){
            if (Input.GetButtonDown("Transformation")){
                if (readyToTransform && alreadyTransformed == false){
                    ParticleControllerScript.Begin(countOfParticles);
                    transformationScript.Start_transformation(other.gameObject);
                    readyToTransformBack = true;
                    readyToTransform = false;
                    alreadyTransformed = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Animal")){
            readyToTransform = false;
            alreadyTransformed = false;
        }
    }
}
