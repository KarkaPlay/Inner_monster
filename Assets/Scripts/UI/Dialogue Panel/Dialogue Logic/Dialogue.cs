using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Dialogue", menuName = "Scriptable Object/Dialogues/NPC Dialogue Sample")]
public class Dialogue : ScriptableObject {
public string npcName;
public Message[] messages;
}
[System.Serializable]
public class Message {
[TextArea(3,15)]
public string text;
public int trigger;
public Response[] responses;
}
[System.Serializable]
public class Response {
public int next;
public string reply;
}