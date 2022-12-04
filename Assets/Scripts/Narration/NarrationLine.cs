using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Line")]
public class NarrationLine : ScriptableObject
{
    [SerializeField]
    private NarratorCharacter m_Speaker; // NPC that is speaking
    [SerializeField]
    private string m_Text; // The text it speaks 

    public NarratorCharacter Speaker => m_Speaker;
    public string Text => m_Text;
}