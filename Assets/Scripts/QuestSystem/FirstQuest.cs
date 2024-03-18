using UnityEngine;

public class FirstQuest : MonoBehaviour
{

    public LootedObj rockLooted;
    public GameObject rockLootedTrigger;
    public MovableObj rockMovable;
    public GameObject rockMovableTrigger;
    public GameObject hiddenPlatform;

    private Quest moveLootedQuest;
    private bool isAllRocksMoved = false;

    private void GiveFirstQuest()
    {
        Vector3 targetPosition = new Vector3(-93.26f, -0.25f, 92.65f);
        Quest reachPointQuest = new Quest("Подойдите к костру", "Встаньте вплотную к костру на локации", QuestIDs.GoToCampfire);
        reachPointQuest.AddCondition(new ReachPointCondition(targetPosition, 5f));
        QuestManager.Instance.AddQuest(reachPointQuest);
        reachPointQuest.StartQuest();
    }


    private void QuestMoveAndLootable()
    {
        Vector3 targetPosition = new Vector3(-57.53f, 5.77f, 67.57f);

        moveLootedQuest = new Quest("Дойти до кубка", "Выполните серию квестов с перемещением камней и доберитесь до кубка", QuestIDs.MoveRocksToBox);
        moveLootedQuest.AddCondition(new MoveLootedCondition(rockLooted, rockLootedTrigger));
        moveLootedQuest.AddCondition(new MoveMovableCondition(rockMovable, rockMovableTrigger));
        moveLootedQuest.AddCondition(new ReachPointCondition(targetPosition, 2f));
        QuestManager.Instance.AddQuest(moveLootedQuest);
        moveLootedQuest.StartQuest();
    }

    private void Start()
    {
        //GiveFirstQuest();
        QuestMoveAndLootable();
    }

    private void Update()
    {
        int i = 0;
        if (!isAllRocksMoved)
        {
            foreach (var condition in moveLootedQuest.Conditions)
            {
                if (condition.CheckCondition())
                {
                    i++;
                }
            }
            if (i == 2)
            {
                isAllRocksMoved = true;
                hiddenPlatform.SetActive(true);
                Debug.Log("Все камни были перенесены, дорога к кубку открыта!");
            }
        }
    }

}