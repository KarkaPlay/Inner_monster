using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class ElderMovement : MonoBehaviour
{
    public NPC npc;

    public GameObject dialogueUI;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogue;
    public TextMeshProUGUI playerResponseQuote;
    public TextMeshProUGUI playerResponseQuote2;
    public Button playerResponse;
    public Button playerResponse2;

    public NavMeshAgent point;
    public GameObject ExitDoor;
    public GameObject EnterDoor;

    private void Awake() {
        ExitTrigger.isEntered = false;
        dialogueUI.SetActive(false);
        TurnOff(playerResponse, playerResponseQuote);
        TurnOff(playerResponse2, playerResponseQuote2);
    }

    public void FixedUpdate(){
        if (ExitTrigger.isEntered == false){
            switch (MonologueTime.time / 300){
            case -3: 
                TurnOff(playerResponse, playerResponseQuote);
                TurnOff(playerResponse2, playerResponseQuote2);
                npcDialogue.text = npc.dialogue[6];
            return;
            case -2:
                npcDialogue.text = npc.dialogue[7];
            return;
            case -1:
                MonologueTime.time = 600;
            return;
            case 1:
                StartDialogue();
                npcDialogue.text = npc.dialogue[0];
            return;
            case 2:
                npcDialogue.text = npc.dialogue[1];
            return;
            case 3:
                npcDialogue.text = npc.dialogue[2];
            return;
            case 4:
                npcDialogue.text = npc.dialogue[3];
            return;
            case 5:
                npcDialogue.text = npc.dialogue[4];
                Destroy(ExitDoor);
            return;
            case 6:
                EndDialogue();
                EnterDoor.SetActive(true);
                point.destination = (new Vector3 (-4.5f, -6.75f, 213.25f));
            return;
            }
        } else {
            npcDialogue.text = npc.dialogue[5];
            TurnOn(playerResponse, playerResponseQuote, npc.playerDialogue[0]);
            TurnOn(playerResponse2, playerResponseQuote2, npc.playerDialogue[1]);
        }
    }

    public void StartDialogue(){
        npcName.text = npc.Name;
        dialogueUI.SetActive(true);
    }

    public void EndDialogue(){
        dialogueUI.SetActive(false);
        playerResponse.interactable = false;
        playerResponse2.interactable = false;
    }

    public void GetResponse(){
        ExitTrigger.isEntered = false;
        MonologueTime.time = -1200;
    }

    public void TurnOff(Button button, TextMeshProUGUI text){
       button.interactable = false;
       text.text = " ";
    }

    public void TurnOn(Button button, TextMeshProUGUI text, string quote){
       button.interactable = true;
       text.text = quote;
    }
}
