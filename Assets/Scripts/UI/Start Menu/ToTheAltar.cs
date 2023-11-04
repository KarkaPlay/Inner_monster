using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToTheAltar : MonoBehaviour
{
    public string Scene;

    public void ToAltar(){
        SceneManager.LoadScene(Scene);
    }

    public void ExitGame(){
        Application.Quit();
    }
}
