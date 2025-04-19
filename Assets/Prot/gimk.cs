using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gimk : MonoBehaviour
{
    public string closedDoorTag = "ClosedDoor";     // 閉まった扉のTag
    public string openDoorTag = "OpenDoor";         // 開いた扉のTag（非表示）

    public GameObject door;             // 実際に見つかった閉じた扉
    public GameObject openDoorObject;   // 実際に見つかった開いた扉

    private bool isPressed = false;

    private void Start()
    {
        // タグで探す（最初に一個だけ見つける）
        door = GameObject.FindWithTag(closedDoorTag);
        openDoorObject = GameObject.FindWithTag(openDoorTag);

        // 念のため開いた扉は非表示にしとく（保険）
        if (openDoorObject != null)
        {
            openDoorObject.SetActive(false);
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
            // 扉の位置を一致させる
            openDoorObject.transform.position = door.transform.position;
            openDoorObject.transform.rotation = door.transform.rotation;
            openDoorObject.transform.localScale = door.transform.localScale;
            openDoorObject.transform.parent = door.transform.parent;

            Destroy(door);
            openDoorObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("ドアが見つからへん！Tag設定されてるか確認してな");
        }
    }
}

