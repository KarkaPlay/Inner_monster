using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMovableCondition : QuestConditionBase
{
    private MovableObj movable;
    private GameObject trigger;
    private Vector3 boxPosition, boxSize;
    private Quaternion boxRotation;

    public MoveMovableCondition(MovableObj movable, GameObject trigger)
    {
        this.movable = movable;
        this.trigger = trigger;
        BoxCollider boxCollider = trigger.GetComponent<BoxCollider>();
        boxSize = boxCollider.size * 0.5f; // ������ ������ ������� �������, ��� ��� Unity ���������� ���������� ������� ��� OverlapBox
        boxRotation = trigger.transform.rotation;
        boxPosition = trigger.transform.position + boxCollider.center; // �������� ���������� ��������������
    }

    public override string Description =>
        $"�������� ������ {movable.movableName} �� ���� {movable.transform.position} � ���� {trigger.transform.position}";

    protected override bool Check()
    {
        Collider[] hitColliders = Physics.OverlapBox(boxPosition, boxSize, boxRotation);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform == movable.transform)
            {
                Debug.Log("�������� ������� ���������");
                return true;
            }
        }
        return false;
    }
}