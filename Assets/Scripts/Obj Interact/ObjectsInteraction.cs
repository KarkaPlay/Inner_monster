using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInteraction : MonoBehaviour
{
    public float minDistanceToObj = 2f;
    public bool isHoldingObject = false; // ����, ������������, ������ �� ����� ������

    public Transform handPosition; // ������� ����, ���� ����� ������������� LootedObj
    public GameObject playerWeapon, playerShield;

    private Transform objectInHand; // ��������� �������, ������� �������� � ����
    private StarterAssets.ThirdPersonController playerController;
    private float defaultPlayerSpeed;
    public float cameraSpeedDivider = 2f; // �������� �������� ������ ����� ����� ���������� ����

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
            ReleaseObject(); // ��������� ������, ���� �� ��� ��������
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
        isHoldingObject = true; // ��������� �����, ��� ������ �������� � ����
        objectInHand = hitCollider.transform; // ���������� ������ �� ��������� ��������� �������
        objectInHand.SetParent(handPosition); // ������������ � ����
        objectInHand.localPosition = looted.handOffsets; // ����� ������� ��� ����������� ����������� � ����
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
        objectInHand.SetParent(null); // ��������� �� ����
        isHoldingObject = false; // ����� ����� �������� �������
        objectInHand = null; // ����� ������ �� ������
        playerWeapon.SetActive(true);
        playerShield.SetActive(true);
        playerController.MoveSpeed = defaultPlayerSpeed;
    }
}