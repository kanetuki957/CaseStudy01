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

    // マウスクリックで選択
    void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                selectedObject = hit.collider.gameObject;
                Debug.Log("Selected: " + selectedObject.name);
            }
        }
    }

    // Ctrl+C でコピー、Ctrl+V でペースト
    void HandleCopyPaste()
    {
        if (selectedObject != null)
        {
            // コピー (Ctrl + C)
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
                copiedObject = selectedObject;
                Debug.Log("Copied: " + copiedObject.name);
            }

            // ペースト (Ctrl + V)
            if (copiedObject != null && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
            {
                Vector3 newPos = copiedObject.transform.position + Vector3.right * 2; // 横にずらして生成
                GameObject newObj = Instantiate(copiedObject, newPos, copiedObject.transform.rotation);
                newObj.name = copiedObject.name + "_Copy";
                Debug.Log("Pasted: " + newObj.name);
            }
        }
    }
}
