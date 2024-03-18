using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLootedCondition : QuestConditionBase
{
    private LootedObj movable;
    private GameObject trigger;
    private Vector3 boxPosition, boxSize;
    private Quaternion boxRotation;

    public MoveLootedCondition(LootedObj movable, GameObject trigger)
    {
        this.movable = movable;
        this.trigger = trigger;
        BoxCollider boxCollider = trigger.GetComponent<BoxCollider>();
        boxSize = boxCollider.size * 0.5f; // Аналогично, размер нужно поделить пополам
        boxRotation = trigger.transform.rotation;
        boxPosition = trigger.transform.position + boxCollider.center; // Корректное преобразование
    }

    public override string Description =>
        $"Возьмите в руки объект {movable.movableName} из зоны {movable.transform.position}, и перенесите его в зону {trigger.transform.position}";

    protected override bool Check()
    {
        Collider[] hitColliders = Physics.OverlapBox(boxPosition, boxSize, boxRotation);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform == movable.transform && !QuestManager.IsPlayerHoldingObj)
            {
                Debug.Log("Перенос объекта выполнен");
                return true;
            }
        }
        return false;
    }
}