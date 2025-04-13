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

    // オブジェクトをクリックして選択（2D対応）
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

    // Ctrl+Cでコピー、Ctrl+Vでマウス位置に複製
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
