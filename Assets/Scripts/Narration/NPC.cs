using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC File", menuName = "NPC Files Words")]
public class NPC : ScriptableObject
{
    public string Name;
    [TextArea(3,15)]
    public string[] dialogue;
    [TextArea(3,15)]
    public int[] nextdialogue;
    [TextArea(1,1)]
    public string[] playerDialogue;
}