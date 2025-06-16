using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPaste2 : MonoBehaviour
{
    private GameObject selectedObject;
    private GameObject copiedObject;
    private bool isDragging = false;

    public GameObject allowedObject;  // コピー可能なオブジェクト（Inspectorから設定）
    private int pasteCount = 0;       // 貼り付けた回数

    void Update()
    {
        HandleSelection();
        HandleCopyPaste();
        HandleDragging();
    }

    // 一回目のクリックで選択、二回目のクリックでドラッグ開始
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
                    isDragging = true; // ドラッグ開始
                }
                else
                {
                    selectedObject = hitObj; // オブジェクト選択
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    // Ctrl+Cでコピー、Ctrl+Vで複製（条件付き）
    void HandleCopyPaste()
    {
        // コピー（指定オブジェクトのみ許可）
        if (selectedObject != null)
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
                if (selectedObject == allowedObject)
                {
                    copiedObject = selectedObject;
                }
            }
        }

        // 貼り付け（最大2回まで）
        if (copiedObject != null && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
        {
            if (pasteCount < 2)
            {
                Vector2 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject newObj = Instantiate(copiedObject, spawnPos, copiedObject.transform.rotation);
                newObj.name = copiedObject.name + "_Copy" + (pasteCount + 1);

                selectedObject = null;
                isDragging = false;
                pasteCount++;
            }
        }
    }

    // ドラッグ処理
    void HandleDragging()
    {
        if (isDragging && selectedObject != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedObject.transform.position = mousePos;
        }
    }
}