using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gimk : MonoBehaviour
{
    public string closedDoorTag = "ClosedDoor";     // �܂�������Tag
    public string openDoorTag = "OpenDoor";         // �J��������Tag�i��\���j

    private GameObject door;             // ���ۂɌ�������������
    private GameObject openDoorObject;   // ���ۂɌ��������J������

    private bool isPressed = false;

    private void Start()
    {
        // �����̎q�I�u�W�F�N�g����Tag�ŒT��
        Transform[] children = GetComponentsInChildren<Transform>(true); // ��A�N�e�B�u���܂߂�
        foreach (Transform child in children)
        {
            if (child.CompareTag(closedDoorTag))
                door = child.gameObject;

            if (child.CompareTag(openDoorTag))
                openDoorObject = child.gameObject;
        }

        // �O�̂��ߊJ�������͔�\���ɂ��Ƃ��i�ی��j
        if (openDoorObject != null)
        {
            openDoorObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("OpenDoorObject��������܂���ł����I");
        }

        if (door == null)
        {
            Debug.LogWarning("Door�i�܂������j��������܂���ł����I");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed && other.CompareTag("Player"))
        {
            isPressed = true;
            ReplaceDoor();
        }
    }

    private void ReplaceDoor()
    {
        if (door != null && openDoorObject != null)
        {
            openDoorObject.transform.position = door.transform.position;
            openDoorObject.transform.rotation = door.transform.rotation;
            openDoorObject.transform.localScale = door.transform.localScale;
            openDoorObject.transform.parent = door.transform.parent;

            Destroy(door);
            openDoorObject.SetActive(true);
        }
    }
}

