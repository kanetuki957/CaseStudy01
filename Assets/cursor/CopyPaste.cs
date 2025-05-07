using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPaste : MonoBehaviour
{
    private GameObject selectedObject;
    private GameObject copiedObject;
    private GameObject clickedObject;

    private bool isDragging = false;

    void Update()
    {
        HandleSelection();
        HandleCopyPaste();
        HandleDragging();
    }

    // ���ڂ̃N���b�N�őI���A���ڂ̃N���b�N�Ńh���b�O
    void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject hitObj = hit.collider.gameObject;

                if (hitObj == selectedObject)
                {
                    // �����I�u�W�F�N�g��2��ڃN���b�N �� �h���b�O�J�n
                    isDragging = true;
                    Debug.Log("Start dragging: " + hitObj.name);
                }
                else
                {
                    // �I�������i1��ڃN���b�N�j
                    selectedObject = hitObj;
                    Debug.Log("Selected: " + selectedObject.name);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
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
                Vector2 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject newObj = Instantiate(copiedObject, spawnPos, copiedObject.transform.rotation);
                newObj.name = copiedObject.name + "_Copy";
                selectedObject = null;     // �\��t������͖��I���ɂ��Ă���
                isDragging = false;        // �����ɂ̓h���b�O���Ȃ�
                Debug.Log("Pasted: " + newObj.name + " at " + spawnPos);
            }
        }
    }

    // �h���b�O���̓}�E�X�ʒu�ɒǏ]
    void HandleDragging()
    {
        if (isDragging && selectedObject != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedObject.transform.position = mousePos;
        }
    }
}
