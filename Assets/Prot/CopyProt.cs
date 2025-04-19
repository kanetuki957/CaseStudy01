using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyProt : MonoBehaviour
{
    public Camera camera1; // �� ��ڂ̃J����
    public Camera camera2; // �� ��ڂ̃J����

    private GameObject selectedObject;
    private GameObject copiedObject;
   
    public GameObject doorPrefab;

    void Update()
    {
        HandleSelection();
        HandleCopyPaste();
    }

    // �I�u�W�F�N�g���N���b�N���đI���i2D�Ή��j
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

    // Ctrl+C�ŃR�s�[�ACtrl+V�Ń}�E�X�ʒu�ɕ���
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
                GameObject newObj = Instantiate(copiedObject, spawnPos, Quaternion.identity);
                newObj.name = copiedObject.name + "_Copy";
                Debug.Log("Pasted: " + newObj.name + " at " + spawnPos);

                // gimk �X�N���v�g������ꍇ�AopenDoorObject ���V�����C���X�^���X�����Đݒ肷��
                gimk originalGimk = copiedObject.GetComponent<gimk>();
                gimk newGimk = newObj.GetComponent<gimk>();

                if (originalGimk != null && newGimk != null)
                {
                    // Project ���̃v���n�u����V�����I�u�W�F�N�g�𐶐�
                    GameObject newDoorObj = Instantiate(doorPrefab);
                    newDoorObj.transform.position = newObj.transform.position;
                    newDoorObj.name = doorPrefab.name + "_Copy";

                    Debug.Log("==> newDoorObj: " + newDoorObj.name);
                    Debug.Log("==> Assigning openDoorObject to newGimk");

                    // �R�s�[���gimk�Ɋ��蓖��
                    newGimk.openDoorObject = newDoorObj;
                    newGimk.door = newObj;
                }
            }
        }
    }

    // �}�E�X�����A�ǂ̃J�����̃r���[�ɂ��邩���肵�ĕԂ�
    Camera GetCameraUnderMouse()
    {
        Vector3 mousePos = Input.mousePosition;

        if (IsMouseInCamera(camera1)) return camera1;
        if (IsMouseInCamera(camera2)) return camera2;

        return null; // �ǂ̃J�����ɂ��܂܂�ĂȂ�
    }

    // �}�E�X���w��J�����̃r���[�|�[�g���ɂ��邩����
    bool IsMouseInCamera(Camera cam)
    {
        Vector3 viewPortPos = cam.ScreenToViewportPoint(Input.mousePosition);
        return viewPortPos.x >= 0 && viewPortPos.x <= 1 && viewPortPos.y >= 0 && viewPortPos.y <= 1;
    }
}
