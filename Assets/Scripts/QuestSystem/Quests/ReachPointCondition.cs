using UnityEngine;


public class ReachPointCondition : QuestConditionBase
{
    private Vector3 targetPoint;
    private float radius;

    public override string Description =>
        $"���������� ����� {targetPoint} � ������� {radius} ������";

    public ReachPointCondition(Vector3 targetPoint, float radius)
    {
        this.targetPoint = targetPoint;
        this.radius = radius;
    }

    protected override bool Check()
    {
        return Vector3.Distance(QuestManager.PlayerTransform.position, targetPoint) <= radius;
    }

    protected override void OnComplete()
    {
        Debug.Log("������� ���������.");
    }
}