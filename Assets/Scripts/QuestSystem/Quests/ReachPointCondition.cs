using UnityEngine;


public class ReachPointCondition : IQuestCondition
{
    public string Description { get; private set; }
    private Vector3 targetPoint;
    private float radius;

    public ReachPointCondition(Vector3 targetPoint, float radius)
    {
        this.targetPoint = targetPoint;
        this.radius = radius;
        Description = $"���������� ����� {targetPoint} � ������� {this.radius}";
    }

    public bool CheckCondition()
    {
        if(Vector3.Distance(QuestManager.PlayerTransform.position, targetPoint) <= radius)
        {
            Debug.Log("������� ���������.");
            return true;
        }
        return false;
    }
}