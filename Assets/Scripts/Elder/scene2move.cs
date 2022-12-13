using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class scene2move : MonoBehaviour
{
    public NPC npc;

    public GameObject dialogueUI;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogue;
    public TextMeshProUGUI playerResponseQuote;
    public TextMeshProUGUI playerResponseQuote2;
    public Button playerResponse;
    public Button playerResponse2;
    public int n=0;
    void Start()
    {
  StartCoroutine(Wait());
       
    }
   
    // Update is called once per frame
    void FixedUpdate()
    {
        switch (n) {
            case 1:
        npcDialogue.text = npc.dialogue[0];
  playerResponseQuote.text = npc.playerDialogue[0];
                n++;
     
                return;
            case 2:

                
                return;
        }
    }
    public void StartDialogue()
    {
        npcName.text = npc.Name;
        dialogueUI.SetActive(true);
    }
    public void EndDialogue()
    {
        dialogueUI.SetActive(false);
        playerResponse.interactable = false;
        playerResponse2.interactable = false;
    }
    public IEnumerator Wait()
{
    yield return new WaitForSeconds(5);
        npcName.text = npc.Name;
        dialogueUI.SetActive(true);
        n++;
    }

}
