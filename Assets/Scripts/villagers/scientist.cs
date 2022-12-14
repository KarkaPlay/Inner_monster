using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public class scientist : MonoBehaviour
{
    public NPC npc;

    public GameObject dialogueUI;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogue;
    public TextMeshProUGUI playerResponseQuote;
    public TextMeshProUGUI playerResponseQuote2;
    public Button playerResponse;
    public Button playerResponse2;
    public int n = 0;
    public int n2 = 0;
    public GameObject tip;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            tip.SetActive(true);
        }
    }
    void OnTriggerStay(Collider other)
    {
        
        if (other.tag == "Player" & Input.GetKeyDown(KeyCode.F) )
        {
            StartDialogue();
            tip.SetActive(false);
            npcDialogue.text = npc.dialogue[n2];
            playerResponseQuote.text = npc.playerDialogue[n2];
         
        }
    }
    void OnTriggerExit(Collider other)
    {
        tip.SetActive(false);

    }
    public void StartDialogue()
    {
        npcName.text = npc.Name;
        dialogueUI.SetActive(true);
    }
    public void EndDialogue()
    {
        dialogueUI.SetActive(false);
        
    }
}
