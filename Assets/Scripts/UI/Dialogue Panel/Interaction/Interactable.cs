using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Interactable : MonoBehaviour

{
    
    // FOR ALL INTERACTABLES //
    [SerializeField]
    UnityEvent m_OnInteraction;

    [SerializeField]
    public int Trigger;

    public void DoInteraction()
    {
        m_OnInteraction.Invoke();
        
    }

    public virtual int TriggerCheck(int Trigger){
        return 0;
    }

    // FOR NPCs ONLY //
    public virtual void QuestCheck(int Trigger){
        
    }
}