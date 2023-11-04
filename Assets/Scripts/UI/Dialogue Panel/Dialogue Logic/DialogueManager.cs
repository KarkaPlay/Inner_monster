using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Manager", menuName = "Scriptable Object/Dialogues/NPC Dialogue Manager")]
public class DialogueManager : ScriptableObject{
    
    public delegate void DialogueCallback(Dialogue dialogue);
    public DialogueCallback OnDialogueStarted;

    public delegate void MonologueCallback(Dialogue dialogue, Interactable Npc);
    public MonologueCallback OnMonologueStarted;

    public delegate void TellingCallBack();
    public TellingCallBack OnDialogueEnded;

    public delegate void ResponsingCallBack(int NextMessageInd);
    public ResponsingCallBack OnResponse;

    public void StartDialogue (Dialogue dialogue){
        OnDialogueStarted.Invoke(dialogue);
    }

    public void StartMonologue (Dialogue dialogue, Interactable Npc){
        OnMonologueStarted.Invoke(dialogue, Npc);
    }

    public void EndDialogue (){
        OnDialogueEnded.Invoke();
    }

    public void GetResponse (int NextMessageInd){
        OnResponse.Invoke(NextMessageInd);
    }
}