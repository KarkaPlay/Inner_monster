using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipsManager : MonoBehaviour
{
    public static Action<string> displayTipEvent;
    public static Action disableTipEvent;
    [SerializeField] private TMP_Text messageText;
     private int activeTips;
    public GameObject tips;

    /*
     public void Show(string message)
     {
         messageText.text = message;
         Tips.SetActive(true);
     }*/

     public void Hide()
     {
         tips.SetActive(false);
     }
    
    public void Start()
    {
        Time.timeScale = 1f;
    }
    private void OnEnable()
    {
        displayTipEvent += displayTip;
        disableTipEvent += disableTip;
    }
    private void OnDisable()
    {
        displayTipEvent -= displayTip;
        disableTipEvent -= disableTip;
    }
    private void displayTip(string message)
    {
        messageText.text = message;
        tips.SetActive(true);
        ++activeTips;
    }
    private void disableTip()
    {
        tips.SetActive(false);
        --activeTips;
    }
    // Start is called before the first frame update
   


    // Update is called once per frame
    void Update()
    {
        
    }
}
