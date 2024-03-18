using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInteraction : MonoBehaviour
{
    public float minDistanceToObj = 2f;
    public bool isHoldingObject = false; // ����, ������������, ������ �� ����� ������
    private Transform objectInHand; // ��������� �������, ������� �������� � ����

    public Transform handPosition; // ������� ����, ���� ����� ������������� LootedObj
    public GameObject playerWeapon, playerShield;

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
    }
}