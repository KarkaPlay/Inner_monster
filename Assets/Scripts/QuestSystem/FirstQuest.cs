using UnityEngine;

public class FirstQuest : MonoBehaviour
{

    private void GiveFirstQuest()
    {
        Vector3 targetPosition = new Vector3(-93.26f, -0.25f, 92.65f);
        Quest reachPointQuest = new Quest("��������� � ������", "�������� �������� � ������ �� �������", QuestIDs.GoToCampfire);
        reachPointQuest.AddCondition(new ReachPointCondition(targetPosition, 5f, QuestManager.PlayerTransform));
        QuestManager.Instance.AddQuest(reachPointQuest);
        reachPointQuest.StartQuest();
    }

    private void Start()
    {
        GiveFirstQuest();
    }

}