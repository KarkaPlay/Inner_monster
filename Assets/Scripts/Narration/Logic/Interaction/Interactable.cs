using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
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

    public abstract int TriggerCheck(int Trigger);

    // FOR NPCs ONLY //
    public abstract void QuestCheck();
}