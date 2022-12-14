using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour
{
    private bool Go = false; //Флаг для начала превращения
    private string[] bodyPart = new string[] {"Body", "Eyes", "Mouth", "Nose", "Horns"};
    private Mesh[] myStartMesh = new Mesh[5];

    private Material[] myBodyMaterials = new Material[3];
    private Material[] myOtherPartsMaterials = new Material[4];

    private AudioSource transformationAudio;
    private void Start(){
        int i = 0;
        foreach (string s in bodyPart){
            SaveCharacteristics(i, s);
                i++;
        }

        transformationAudio = GetComponent<AudioSource>();
    }

    private void SaveCharacteristics(int i, string bodyPart){
        SkinnedMeshRenderer targetPart = transform.Find(bodyPart).GetComponent<SkinnedMeshRenderer>();

        myStartMesh[i] = targetPart.sharedMesh;

        if (bodyPart == "Body"){
            myBodyMaterials = targetPart.materials;
        }
        else{
            myOtherPartsMaterials[i-1] = targetPart.sharedMaterial;
        }
    }
    
    public void Start_transformation(GameObject Target){
        Debug.Log("Loh");
        
        //Запускаем звук превращения
        transformationAudio.Play();
        
        //Запоминаем рендер цели
        /*SkinnedMeshRenderer otherBody = Target.transform.Find("Body").GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer otherEyes = Target.transform.Find("Eyes").GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer otherMouth = Target.transform.Find("Mouth").GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer otherNose = Target.transform.Find("Nose").GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer otherHorns = Target.transform.Find("Horns").GetComponent<SkinnedMeshRenderer>();*/

        foreach (string i in bodyPart){
            //Изменяем наш меш на меш цели
            ChangeMesh(i, Target);
            //Обмениваемся материалами
            ChangeMaterial(i, Target);
        }

   }
    
    //метод копирует меш
    private void ChangeMesh(string bodyPart, GameObject Target){
        transform.Find(bodyPart).GetComponent<SkinnedMeshRenderer>().sharedMesh = Target.transform.Find(bodyPart).GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }
    //Метод копирует материалы
    private void ChangeMaterial(string bodyPart, GameObject Target){
        SkinnedMeshRenderer other = Target.transform.Find(bodyPart).GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer player = transform.Find(bodyPart).GetComponent<SkinnedMeshRenderer>();
        player.materials = other.materials;
    }

    public void Transform_back(){
        Debug.Log("Hehe");
        
        //Запускаем звук превращения
        transformationAudio.Play();
        
        int i = 0;
        foreach (string s in bodyPart){
            //Изменяем наш меш
            TransformBackMesh(i, s);
            //Обмениваемся материалами
            TransformBackMaterials(i, s);
            i++;
        }
    }

    private void TransformBackMesh(int i, string bodyPart){
        transform.Find(bodyPart).GetComponent<SkinnedMeshRenderer>().sharedMesh = myStartMesh[i];
    }

    private void TransformBackMaterials(int i, string bodyPart){
        SkinnedMeshRenderer targetPart = transform.Find(bodyPart).GetComponent<SkinnedMeshRenderer>();
        if (bodyPart == "Body"){
            targetPart.materials = myBodyMaterials;
        }
        else{
            targetPart.sharedMaterial = myOtherPartsMaterials[i-1];
        }
        //transform.Find(bodyPart).GetComponent<SkinnedMeshRenderer>().materials = myStartMaterials[i];
    }

}
