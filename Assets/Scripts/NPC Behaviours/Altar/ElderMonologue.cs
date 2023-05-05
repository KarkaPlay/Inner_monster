using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderMonologue : MonoBehaviour
{
    private IEnumerator coroutine;

    [SerializeField]
    private DialogueManager m_DialogueManager;

    [SerializeField]
    private Dialogue dialogue;

    [SerializeField]
    public Interactable Npc;


    void Start()
    {
        coroutine = MonoStart(2f);
        StartCoroutine(coroutine);
        coroutine = MonoCont(2f);
        StartCoroutine(coroutine);
    }

    IEnumerator MonoStart(float time)
    {
        while(true){
            yield return new WaitForSeconds(time);
            m_DialogueManager.StartMonologue(dialogue, Npc);
        }
    }

    IEnumerator MonoCont(float time)
    {
        while(true){
            yield return new WaitForSeconds(time);
            //m_DialogueManager.GetResponse(dialogue.messages);
        }
    }
}
