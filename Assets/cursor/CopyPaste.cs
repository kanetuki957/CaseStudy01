using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPaste : MonoBehaviour
{
    private GameObject selectedObject;
    private GameObject copiedObject;

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
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
                Vector2 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject newObj = Instantiate(copiedObject, spawnPos, Quaternion.identity);
                newObj.name = copiedObject.name + "_Copy";
                Debug.Log("Pasted: " + newObj.name + " at " + spawnPos);
            }
        }
    }
}
