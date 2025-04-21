using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyProt : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    private GameObject selectedObject;
    private GameObject copiedObject;

    void Update()
    {
        HandleSelection();
        HandleCopyPaste();
    }

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
                GameObject newObj = Instantiate(copiedObject, spawnPos, Quaternion.identity);
                newObj.name = copiedObject.name + "_Copy";

                Debug.Log("Pasted: " + newObj.name + " at " + spawnPos);

                // gimkは自身の子オブジェクトからタグで探す設計なので、特に設定しなくてOK
            }
        }
    }

    Camera GetCameraUnderMouse()
    {
        Vector3 mousePos = Input.mousePosition;

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
