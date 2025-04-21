using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenDoor : MonoBehaviour, IActivatable
{
    public GameObject door;  // 扉のGameObject
    public Sprite openDoorSprite; // 開いた扉の画像
    private SpriteRenderer doorSpriteRenderer;
    private BoxCollider2D doorCollider;

    void Start()
    {
        if (door != null)
        {
            doorSpriteRenderer = door.GetComponent<SpriteRenderer>(); // 扉のスプライトレンダラーを取得
            doorCollider = door.GetComponent<BoxCollider2D>(); // 扉のコライダーを取得
        }
    }

    public void Activate()
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
