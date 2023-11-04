using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnterTrigger : MonoBehaviour
{    
    public string Scene;

    void OnTriggerEnter(Collider other) { 
        if (other.gameObject.tag == "Player"){
            SceneManager.LoadScene(Scene);
        }
    }
}
