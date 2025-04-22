using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class replace : MonoBehaviour
{
    [Header("�J�����ݒ�")]
    public Camera camera1;
    public Camera camera2;

    [Header("�R�s�[�������̐e�I�u�W�F�N�g")]
    public Transform mainBody;

    [Header("���̃^�O�ݒ�")]
    public string closedDoorTag = "ClosedDoor";
    public string openDoorTag = "OpenDoor";

    private GameObject selectedObject;
    private GameObject copiedObject;

    private bool isPressed = false;
    private GameObject door;
    private GameObject openDoorObject;

    void Awake()
    {
        InitializeDoors();
    }

    void Update()
    {
        HandleSelection();
        HandleCopyPaste();
    }

    //============================
    // �y���̏����F�J�z
    //============================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed && other.CompareTag("Player"))
        {
            isPressed = true;
            ReplaceDoor();
        }
    }

    void InitializeDoors()
    {
        // �q�I�u�W�F�N�g����Tag��������
        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.CompareTag(closedDoorTag))
                door = child.gameObject;

            if (child.CompareTag(openDoorTag))
                openDoorObject = child.gameObject;
        }

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

    void ReplaceDoor()
    {
        if (door != null && openDoorObject != null)
        {
            // �e�𑵂���i���[�J����ňʒu���킹�j
            openDoorObject.transform.position = door.transform.position;
            openDoorObject.transform.rotation = door.transform.rotation;
            openDoorObject.transform.localScale = door.transform.localScale;
            openDoorObject.transform.parent = door.transform.parent;

            // �����ڂ����؂�ւ���i�폜���Ȃ��j
            door.SetActive(false);
            openDoorObject.SetActive(true);
        }
    }

    //============================
    // �y�I�u�W�F�N�g�I���ƃR�s�[�z
    //============================
    void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = GetCameraUnderMouse();
            if (cam == null) return;

            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                selectedObject = hit.collider.gameObject;
                Debug.Log("Selected: " + selectedObject.name);
            }
        }
    }

    void HandleCopyPaste()
    {
        if (selectedObject != null)
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
                copiedObject = selectedObject;
                Debug.Log("Copied: " + copiedObject.name);
            }

            if (copiedObject != null && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
            {
                Camera cam = GetCameraUnderMouse();
                if (cam == null) return;

                Vector2 spawnPos = cam.ScreenToWorldPoint(Input.mousePosition);
                GameObject newObj = Instantiate(copiedObject, spawnPos, Quaternion.identity, mainBody);
                newObj.transform.localScale = copiedObject.transform.localScale;
                newObj.name = copiedObject.name + "_Copy";

                Debug.Log("Pasted: " + newObj.name + " at " + spawnPos);
                // trigger �ɐ����� targetBridge ��ݒ肷��

                trigger[] triggers = newObj.GetComponentsInChildren<trigger>();
                foreach (trigger t in triggers)
                {
                    t.targetBridge = newObj.GetComponent<replace>();
                }
                // �J�������������I�ɔ�\���ɂ��A������Ԃ𐮂���
                replace bc = newObj.GetComponent<replace>();
                if (bc != null)
                {
                    bc.ResetBridgeState();
                }
            }
        }
    }

    void ResetBridgeState()
    {
        isPressed = false;
        InitializeDoors();
    }
    public void TriggerOpen()
    {
        if (!isPressed)
        {
            isPressed = true;
            ReplaceDoor();
        }
    }
    Camera GetCameraUnderMouse()
    {
        if (IsMouseInCamera(camera1)) return camera1;
        if (IsMouseInCamera(camera2)) return camera2;
        return null;
    }

    bool IsMouseInCamera(Camera cam)
    {
        Vector3 viewPortPos = cam.ScreenToViewportPoint(Input.mousePosition);
        return viewPortPos.x >= 0 && viewPortPos.x <= 1 && viewPortPos.y >= 0 && viewPortPos.y <= 1;
    }
}
