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

    public GameObject dialogueUI;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogue;
    public TextMeshProUGUI playerResponseQuote;
    public TextMeshProUGUI playerResponseQuote2;
    public Button playerResponse;
    public Button playerResponse2;
    public NavMeshAgent goat;
    private int k = 0;
    void Start()
    {
  StartCoroutine(Wait());
       
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"&& trig.n == 2)
        {
            trig.contact = true;
        }
        if (other.tag == "Player" && trig.n == 5)
        {
            trig.contact = true;
        }
        if (other.tag == "Player" && trig.n == 8)
        {
            trig.contact = true;
        }
        if (other.tag == "Player" && trig.n == 9)
        {
            trig.contact = true;
        }
        if (other.tag == "Player" && trig.n == 10)
        {
            trig.contact = true;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(trig.n);
        switch (trig.n) {
            case 1:
        npcDialogue.text = npc.dialogue[0];
  playerResponseQuote.text = npc.playerDialogue[0];
                StartCoroutine(WaitN());
                return;
            case 2:
                // transform.position = new Vector3(-55 , 2, 94);
                goat.destination = new Vector3(-66, 2, 92);
                if (trig.contact == true) {
                    trig.n++; trig.contact = false;
                }
                return;
            case 3:
                trig.contact = false;
             
                if (k == 0) { dialogueUI.SetActive(true); k++; }
                npcDialogue.text = npc.dialogue[1];
                playerResponseQuote.text = npc.playerDialogue[1];   
                return;
            case 4:
                 dialogueUI.SetActive(true);
                npcDialogue.text = npc.dialogue[2];
                playerResponseQuote.text = npc.playerDialogue[2];
                trig.n++; trig.contact = false;
                return;
            case 5:
                goat.destination = new Vector3(-111, 0, 80);
                if (trig.contact == true)
                {
                    trig.n++; trig.contact = false;
                }
                return;
            case 6:
                dialogueUI.SetActive(true); 
                npcDialogue.text = npc.dialogue[3];
                playerResponseQuote.text = npc.playerDialogue[3];
                trig.n++;
                trig.contact = false;
                return;
            case 7:
                StartCoroutine(Move());  break;
            case 8:
                if (trig.contact == true)
                {
                    dialogueUI.SetActive(true);
                    npcDialogue.text = npc.dialogue[4];
                    playerResponseQuote.text = npc.playerDialogue[4];
                    trig.contact = false;
                    trig.n++;
                }return;
            case 9:
                goat.destination = new Vector3(-106, 2, 166);
                if (trig.contact == true)
                {
                    trig.n++; trig.contact = false;
                }
              
                return;
            case 10:
                if (trig.contact == true)
                {
                    dialogueUI.SetActive(true);
                    npcDialogue.text = npc.dialogue[5];
                    playerResponseQuote.text = npc.playerDialogue[5];
                }
                return;
        }
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
