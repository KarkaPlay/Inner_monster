using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueMenuController : MonoBehaviour
{

    [SerializeField]
    private DialogueManager m_DialogueManager;
    [SerializeField]
    private TextMeshProUGUI m_NpcName;
    [SerializeField]
    private TextMeshProUGUI m_NpcDialogueText;

    [SerializeField]
    private RectTransform m_ResponsesBoxTransform;
    [SerializeField]
    private DialogueResponseController m_ResponseControllerPrefab;

    [SerializeField]
    private Interactable m_Npc;
    [SerializeField]
    //private GameObject m_Player;

    private Dialogue CurrentDialogue;
    private Message CurrentMessage;
    private int NextMessageInd;
    private int ResponceInd;
    private int Trigger;

    private void Awake(){
        gameObject.SetActive(false);
        m_DialogueManager.OnDialogueStarted += OnDialogueStarted;
        m_DialogueManager.OnMonologueStarted += OnMonologueStarted;
        m_DialogueManager.OnDialogueEnded += OnDialogueEnded;
        m_DialogueManager.OnResponse += OnResponse;
    }

    private void OnDialogueStarted(Dialogue dialogue)
    {
        m_Npc = InteractionFinder.m_NearbyInteractables[0];
        Trigger = m_Npc.Trigger;
        gameObject.SetActive(true);

        CurrentDialogue = dialogue;
        m_NpcName.text = CurrentDialogue.npcName;
        DialogueContinue(CurrentDialogue, m_Npc.TriggerCheck(Trigger));
    }

    // TEST //
    private void OnMonologueStarted(Dialogue dialogue, Interactable Npc)
    {
        m_Npc = Npc;
        Trigger = m_Npc.Trigger;
        gameObject.SetActive(true);

        CurrentDialogue = dialogue;
        m_NpcName.text = CurrentDialogue.npcName;
        DialogueContinue(CurrentDialogue, m_Npc.TriggerCheck(Trigger));
    }
    //
    
    private void DialogueContinue(Dialogue dialogue, int ResponseInd){
        m_NpcDialogueText.text = CurrentDialogue.messages[ResponseInd].text;
        m_Npc.Trigger = CurrentDialogue.messages[ResponseInd].trigger;
        CurrentMessage = CurrentDialogue.messages[ResponseInd];
        ResponseCheck();
    }

    private void OnDialogueEnded()
    {
        gameObject.SetActive(false);
    }

    private void OnResponse(int ResponseInd)
    {
        foreach (Transform child in m_ResponsesBoxTransform)
        {
            Destroy(child.gameObject);
        }
        if (ResponseInd == -1) {
            m_DialogueManager.OnDialogueEnded();
        } else {
        DialogueContinue(CurrentDialogue, ResponseInd);
        };
    }

    private void ResponseCheck(){
        foreach (Response response in CurrentMessage.responses){
            DialogueResponseController newResponse = Instantiate(m_ResponseControllerPrefab, m_ResponsesBoxTransform);
            newResponse.Resp = response; 
        }
    }
}