using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gimk : MonoBehaviour
{
    public string closedDoorTag = "ClosedDoor";     // 閉まった扉のTag
    public string openDoorTag = "OpenDoor";         // 開いた扉のTag（非表示）

    private GameObject door;             // 実際に見つかった閉じた扉
    private GameObject openDoorObject;   // 実際に見つかった開いた扉

    private bool isPressed = false;

    private void Start()
    {
        // 自分の子オブジェクトからTagで探す
        Transform[] children = GetComponentsInChildren<Transform>(true); // 非アクティブも含める
        foreach (Transform child in children)
        {
            if (child.CompareTag(closedDoorTag))
                door = child.gameObject;

            if (child.CompareTag(openDoorTag))
                openDoorObject = child.gameObject;
        }

        // 念のため開いた扉は非表示にしとく（保険）
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed && other.CompareTag("Player"))
        {
            isPressed = true;
            ReplaceDoor();
        }
    }

    private void ReplaceDoor()
    {
        if (door != null && openDoorObject != null)
        {
            openDoorObject.transform.position = door.transform.position;
            openDoorObject.transform.rotation = door.transform.rotation;
            openDoorObject.transform.localScale = door.transform.localScale;
            openDoorObject.transform.parent = door.transform.parent;

            Destroy(door);
            openDoorObject.SetActive(true);
        }
    }
}

