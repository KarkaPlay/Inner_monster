using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public GameObject hint;
    public bool isPlayerNear = false;
    public void Show()
    {
        hint.SetActive(true);
    }
    public void Hide()
    {
        if(isPlayerNear == true || isPlayerNear == false) {hint.SetActive(false); }       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerNear = false;
    }

    private void Update()
    {
        if (isPlayerNear == true)
        {
            Show();
        }
    }
}
