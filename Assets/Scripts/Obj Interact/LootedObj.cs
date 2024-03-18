using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootedObj : MonoBehaviour
{
    public Vector3 handOffsets;
    public string movableName = "Камень";
    public float speedDivider = 2f; // снижает скорость игрока в x раз во время удерживания
}
