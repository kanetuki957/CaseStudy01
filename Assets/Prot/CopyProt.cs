using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyProt : MonoBehaviour
{
    public Camera camera1; // ← 一つ目のカメラ
    public Camera camera2; // ← 二つ目のカメラ

    private GameObject selectedObject;
    private GameObject copiedObject;
   
    public GameObject doorPrefab;

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
                Camera cam = GetCameraUnderMouse();
                if (cam == null) return;

                Vector2 spawnPos = cam.ScreenToWorldPoint(Input.mousePosition);
                GameObject newObj = Instantiate(copiedObject, spawnPos, Quaternion.identity);
                newObj.name = copiedObject.name + "_Copy";
                Debug.Log("Pasted: " + newObj.name + " at " + spawnPos);

                // gimk スクリプトがある場合、openDoorObject も新しくインスタンス化して設定する
                gimk originalGimk = copiedObject.GetComponent<gimk>();
                gimk newGimk = newObj.GetComponent<gimk>();

                if (originalGimk != null && newGimk != null)
                {
                    // Project 内のプレハブから新しいオブジェクトを生成
                    GameObject newDoorObj = Instantiate(doorPrefab);
                    newDoorObj.transform.position = newObj.transform.position;
                    newDoorObj.name = doorPrefab.name + "_Copy";

                    Debug.Log("==> newDoorObj: " + newDoorObj.name);
                    Debug.Log("==> Assigning openDoorObject to newGimk");

                    // コピー先のgimkに割り当て
                    newGimk.openDoorObject = newDoorObj;
                    newGimk.door = newObj;
                }
            }
        }
    }

    // マウスが今、どのカメラのビューにあるか判定して返す
    Camera GetCameraUnderMouse()
    {
        Vector3 mousePos = Input.mousePosition;

        if (IsMouseInCamera(camera1)) return camera1;
        if (IsMouseInCamera(camera2)) return camera2;

        return null; // どのカメラにも含まれてない
    }

    // マウスが指定カメラのビューポート内にあるか判定
    bool IsMouseInCamera(Camera cam)
    {
        Vector3 viewPortPos = cam.ScreenToViewportPoint(Input.mousePosition);
        return viewPortPos.x >= 0 && viewPortPos.x <= 1 && viewPortPos.y >= 0 && viewPortPos.y <= 1;
    }
}
