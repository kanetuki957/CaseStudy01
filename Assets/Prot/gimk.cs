using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gimk : MonoBehaviour
{
    public string closedDoorTag = "ClosedDoor";     // �܂�������Tag
    public string openDoorTag = "OpenDoor";         // �J��������Tag�i��\���j

    public GameObject door;             // ���ۂɌ�������������
    public GameObject openDoorObject;   // ���ۂɌ��������J������

    private bool isPressed = false;

    private void Start()
    {
        // �^�O�ŒT���i�ŏ��Ɉ����������j
        door = GameObject.FindWithTag(closedDoorTag);
        openDoorObject = GameObject.FindWithTag(openDoorTag);

        // �O�̂��ߊJ�������͔�\���ɂ��Ƃ��i�ی��j
        if (openDoorObject != null)
        {
            openDoorObject.SetActive(false);
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
            // ���̈ʒu����v������
            openDoorObject.transform.position = door.transform.position;
            openDoorObject.transform.rotation = door.transform.rotation;
            openDoorObject.transform.localScale = door.transform.localScale;
            openDoorObject.transform.parent = door.transform.parent;

            Destroy(door);
            openDoorObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("�h�A��������ւ�ITag�ݒ肳��Ă邩�m�F���Ă�");
        }
    }
}

