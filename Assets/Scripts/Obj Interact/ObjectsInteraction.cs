using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInteraction : MonoBehaviour
{
    public float minDistanceToObj = 2f;
    public bool isHoldingObject = false; // Флаг, показывающий, держит ли игрок объект

    public Transform handPosition; // Позиция руки, куда будет прикрепляться LootedObj
    public GameObject playerWeapon, playerShield;

    private Transform objectInHand; // Трансформ объекта, который держится в руке
    private StarterAssets.ThirdPersonController playerController;
    private float defaultPlayerSpeed;
    public float cameraSpeedDivider = 2f; // снижение скорости камеры когда игрок удерживает итем

    private void Start()
    {
        playerController = GetComponent<StarterAssets.ThirdPersonController>();
        defaultPlayerSpeed = playerController.MoveSpeed;
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return;

        if (isHoldingObject)
        {
            ReleaseObject(); // Отпустить объект, если он уже держится
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, minDistanceToObj);
        foreach (var hitCollider in hitColliders)
        {
            if (!isHoldingObject)
            {
                TryToMoveObject(hitCollider);
                TryToLootObject(hitCollider);
            }
        }
    }

    void TryToMoveObject(Collider hitCollider)
    {
        MovableObj movable = hitCollider.GetComponent<MovableObj>();
        if (movable == null) return;
        Rigidbody rb = hitCollider.attachedRigidbody;
        if (rb == null) return;
        Vector3 pushDirection = transform.forward;
        rb.AddForce(pushDirection * movable.pushForce, ForceMode.VelocityChange);
    }

    void TryToLootObject(Collider hitCollider)
    {
        LootedObj looted = hitCollider.GetComponent<LootedObj>();
        if (looted == null) return;
        isHoldingObject = true; // Установка флага, что объект держится в руке
        objectInHand = hitCollider.transform; // Сохранение ссылки на трансформ держимого объекта
        objectInHand.SetParent(handPosition); // Прикрепление к руке
        objectInHand.localPosition = looted.handOffsets; // Сброс позиции для корректного отображения в руке
        playerWeapon.SetActive(false);
        playerShield.SetActive(false);
        Rigidbody lootedRb = hitCollider.GetComponent<Rigidbody>();
        lootedRb.isKinematic = true;
        playerController.MoveSpeed /= looted.speedDivider;
    }

    void ReleaseObject()
    {
        if (objectInHand == null) return;
        Rigidbody lootedRb = objectInHand.GetComponent<Rigidbody>();
        lootedRb.isKinematic = false;
        objectInHand.SetParent(null); // Открепить от руки
        isHoldingObject = false; // Сброс флага держания объекта
        objectInHand = null; // Сброс ссылки на объект
        playerWeapon.SetActive(true);
        playerShield.SetActive(true);
        playerController.MoveSpeed = defaultPlayerSpeed;
    }
}