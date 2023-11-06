using UnityEngine;


public class ReachPointCondition : IQuestCondition
{
    public string Description { get; private set; }
    private Vector3 targetPoint;
    private float radius;
    private Transform playerTransform;

    public ReachPointCondition(Vector3 targetPoint, float radius, Transform playerTransform)
    {
        this.targetPoint = targetPoint;
        this.radius = radius;
        this.playerTransform = playerTransform;
        Description = $"���������� ����� {targetPoint} � ������� {this.radius}";
    }

    public bool CheckCondition()
    {
        if(Vector3.Distance(playerTransform.position, targetPoint) <= radius)
        {
            Debug.Log("������� ���������.");
            return true;
        }
        return false;
    }
}