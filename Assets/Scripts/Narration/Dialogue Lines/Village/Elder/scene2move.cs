using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public class trig
{
    public static bool contact = false;
    public static int n;
}
public class scene2move : MonoBehaviour
{
    public NPC npc;
    public GameObject player;
    
    public GameObject dialogueUI;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogue;
    public TextMeshProUGUI playerResponseQuote;
    public TextMeshProUGUI playerResponseQuote2;
    public Button playerResponse;
    public Button playerResponse2;
    public NavMeshAgent goat;
    private int k = 0;
    
    [SerializeField]private float distance = 0;
    private bool needToWait = false;
    
    void Start()
    { 
        StartCoroutine(Wait());
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void WaitForPlayer()
    {
        if (!needToWait) return;
        distance = Vector3.Distance(transform.position, player.transform.position); 
        
        if (distance < 10) goat.speed = 3.5f;
        else if (distance is < 20 and >= 10) goat.speed = 1.0f;
        else goat.speed = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        var triggers = new List<int>() { 2, 5, 8, 9, 10 };
        
        if (other.CompareTag("Player") && triggers.Contains(trig.n)) 
        { 
            trig.contact = true;
        }
    }
    
    void FixedUpdate()
    {
        WaitForPlayer();
        
        switch (trig.n) {
            case 1:
                SetDialogueText(0, false);
                StartCoroutine(WaitN());
                return;
            case 2:
                // transform.position = new Vector3(-55 , 2, 94);
                goat.destination = new Vector3(-66, 2, 92);
                if (trig.contact)
                { 
                    trig.n++;
                    trig.contact = false;
                }
                return;
            case 3:
                trig.contact = false;
                if (k == 0)
                {
                    dialogueUI.SetActive(true); 
                    k++;
                }
                SetDialogueText(1, false); 
                return;
            case 4:
                SetDialogueText(2);
                trig.n++; trig.contact = false;
                return;
            case 5:
                goat.destination = new Vector3(-111, 0, 80);
                if (trig.contact)
                {
                    trig.n++; trig.contact = false;
                }
                return;
            case 6:
                SetDialogueText(3);
                trig.n++; trig.contact = false;
                return;
            case 7:
                StartCoroutine(Move());  break;
            case 8:
                if (trig.contact)
                {
                    SetDialogueText(4);
                    trig.contact = false; trig.n++;
                }
                return;
            case 9:
                goat.destination = new Vector3(-106, 2, 166);
                if (trig.contact)
                {
                    trig.n++; trig.contact = false;
                }
                return;
            case 10:
                if (trig.contact) SetDialogueText(5);
                return;
        }
    }
    private void SetDialogueText(int dialogueIndex, bool setUI = true)
    {
        if (setUI) dialogueUI.SetActive(true);
        npcDialogue.text = npc.dialogue[dialogueIndex]; 
        playerResponseQuote.text = npc.playerDialogue[dialogueIndex];
        needToWait = true;
    }
    public void StartDialogue()
    {
        npcName.text = npc.Name;
        dialogueUI.SetActive(true);
    }
    public void EndDialogue()
    {
        dialogueUI.SetActive(false);
    }
    public IEnumerator Wait()
{
    yield return new WaitForSeconds(5);
        npcName.text = npc.Name;
        dialogueUI.SetActive(true);
        trig.n++;
}
    public IEnumerator WaitN()
    { 
        yield return new WaitForSeconds(5);
        trig.n = 2;
    }
    public IEnumerator Move()
    {
        yield return new WaitForSeconds(5);
        goat.destination = new Vector3(-137, 0, 141) ;
        trig.n = 8;
    }

}
