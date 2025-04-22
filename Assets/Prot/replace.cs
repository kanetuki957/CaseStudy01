using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class replace : MonoBehaviour
{
    [Header("カメラ設定")]
    public Camera camera1;
    public Camera camera2;

    [Header("コピーした橋の親オブジェクト")]
    public Transform mainBody;

    [Header("扉のタグ設定")]
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
    // 【橋の処理：開閉】
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
        // 子オブジェクトからTagを見つける
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
            Debug.LogWarning("OpenDoorObjectが見つかりませんでした！");
        }

        if (door == null)
        {
            Debug.LogWarning("Door（閉まった扉）が見つかりませんでした！");
        }
    }

    void ReplaceDoor()
    {
        if (door != null && openDoorObject != null)
        {
            // 親を揃える（ローカル基準で位置合わせ）
            openDoorObject.transform.position = door.transform.position;
            openDoorObject.transform.rotation = door.transform.rotation;
            openDoorObject.transform.localScale = door.transform.localScale;
            openDoorObject.transform.parent = door.transform.parent;

            // 見た目だけ切り替える（削除しない）
            door.SetActive(false);
            openDoorObject.SetActive(true);
        }
    }

    //============================
    // 【オブジェクト選択とコピー】
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
                // trigger に正しい targetBridge を設定する

                trigger[] triggers = newObj.GetComponentsInChildren<trigger>();
                foreach (trigger t in triggers)
                {
                    t.targetBridge = newObj.GetComponent<replace>();
                }
                // 開いた扉を強制的に非表示にし、初期状態を整える
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
