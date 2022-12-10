using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// [CreateAssetMenu(fileName = "Elder Monologue Hardcode", menuName = "NPC Files Words/Elder Monologue Hardcode")]
public class ElderMovement : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent point;
    

    public NPC npc;

    public GameObject dialogueUI;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogue;
    public TextMeshProUGUI playerResponseQuote;
    public TextMeshProUGUI playerResponseQuote2;
    public Button playerResponse;
    public Button playerResponse2;


    public GameObject Gate;
    public bool NotInterrupted;

    private void Awake() {
        NotInterrupted = true;
        dialogueUI.SetActive(false);
        TurnOff(playerResponse, playerResponseQuote);
        TurnOff(playerResponse2, playerResponseQuote2);
    }

    public void FixedUpdate(){
        if (NotInterrupted){
            switch (MonologueTime.time / 600){
            case 1:
                StartDialogue();
                npcDialogue.text = npc.dialogue[0];
            return;
            case 2:
                npcDialogue.text = npc.dialogue[1];
                point.destination = (new Vector3(13.5f, -5.5f, 215.25f));
                transform.LookAt(new Vector3(13.5f, 5.2f, 210));
            return;
            case 3:
                npcDialogue.text = npc.dialogue[2];
                point.destination = (new Vector3(5.25f, -5.5f, 215f));
                transform.LookAt(new Vector3(1, -0.2f, 208));
            return;
            case 4:
                npcDialogue.text = npc.dialogue[3];
                point.destination = (new Vector3(-10, -6.5f, 228));
                transform.LookAt(new Vector3(-16, -7.5f, 231));
            return;
            case 5:
                npcDialogue.text = npc.dialogue[4];
                point.destination = (new Vector3(-3.5f, -6, 237));
            return;
            case 6:
                EndDialogue();
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

    public void TurnOff(Button button, TextMeshProUGUI text){
       button.interactable = false;
       text.text = " ";
    }

    public void TurnOn(Button button, TextMeshProUGUI text, string quote){
       button.interactable = true;
       text.text = quote;
    }
}
