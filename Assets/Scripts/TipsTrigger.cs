using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsTrigger : MonoBehaviour
{
   
    [Header("Текст подсказки")]
    [TextArea(3,10)]
    [SerializeField] private string message;
   // public Hint screenfader;
    
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TipsManager.displayTipEvent?.Invoke(message);
            Time.timeScale = 0f;
           // screenfader.fadeState = Hint.FadeState.In;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TipsManager.disableTipEvent?.Invoke();
        }
    }
}
