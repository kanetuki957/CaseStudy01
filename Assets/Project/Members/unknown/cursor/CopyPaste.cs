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

    // 一回目のクリックで選択、二回目のクリックでドラッグ
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
                    // 同じオブジェクトを2回目クリック → ドラッグ開始
                    isDragging = true;
                    Debug.Log("Start dragging: " + hitObj.name);
                }
                else
                {
                    // 選択だけ（1回目クリック）
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
                GameObject newObj = Instantiate(copiedObject, spawnPos, copiedObject.transform.rotation);
                newObj.name = copiedObject.name + "_Copy";
                selectedObject = null;     // 貼り付け直後は未選択にしておく
                isDragging = false;        // すぐにはドラッグしない
                Debug.Log("Pasted: " + newObj.name + " at " + spawnPos);
            }
        }
    }

    // ドラッグ中はマウス位置に追従
    void HandleDragging()
    {
        if (isDragging && selectedObject != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedObject.transform.position = mousePos;
        }
    }
}
