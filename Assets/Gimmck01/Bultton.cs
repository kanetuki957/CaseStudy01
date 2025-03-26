using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bultton : MonoBehaviour
{
    public GameObject door;  // 扉のGameObject
    public Sprite openDoorSprite; // 開いた扉の画像
    private SpriteRenderer doorSpriteRenderer;
    private BoxCollider2D doorCollider;
    private Vector3 originalPosition;
    private bool isPressed = false;

    void Start()
    {
        originalPosition = transform.position; // 初期位置を記憶

        if (door != null)
        {
            doorSpriteRenderer = door.GetComponent<SpriteRenderer>(); // 扉のスプライトレンダラーを取得
            doorCollider = door.GetComponent<BoxCollider2D>(); // 扉のコライダーを取得
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed && other.CompareTag("Player")) // プレイヤーが踏んだら作動
        {
            isPressed = true;
            transform.position -= new Vector3(0, 0.3f, 0); // ボタンを沈める
            OpenDoor(); // 扉を開く（画像変更＋コライダー削除）
        }
    }

    private void OpenDoor()
    {
        if (doorSpriteRenderer != null && openDoorSprite != null)
        {
            doorSpriteRenderer.sprite = openDoorSprite; // 扉の画像を開いた状態に変更
        }

        if (doorCollider != null)
        {
            Destroy(doorCollider); // 扉のコライダーを削除
        }
    }
}
