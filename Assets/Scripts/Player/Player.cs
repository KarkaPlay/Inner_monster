using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private void GiveFirstQuest()
    {
        Vector3 targetPosition = new Vector3(-93.26f, -0.25f, 92.65f);
        Quest reachPointQuest = new Quest("Подойдите к костру", "Встаньте вплотную к костру на локации", QuestIDs.GoToCampfire);
        reachPointQuest.AddCondition(new ReachPointCondition(targetPosition, 5f, transform));
        QuestManager.Instance.AddQuest(reachPointQuest);
        reachPointQuest.StartQuest();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        GiveFirstQuest();
    }

    /*private void Update()
    {
        Debug.Log(transform.position);
    }*/
}