using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenDoor : MonoBehaviour, IActivatable
{
    public GameObject door;  // 扉のGameObject
    public Sprite openDoorSprite; // 開いた扉の画像
    public Sprite closeDoorSprite;
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
        if (doorCollider.enabled)   // 扉が閉じているなら
        {
            doorSpriteRenderer.sprite = openDoorSprite; // 扉の画像を開いた状態に変更

            doorCollider.enabled = false; // 扉のコライダーを無効化
            if (doorCollider.enabled == true)
            {
            }
        }
        else
        {
            doorSpriteRenderer.sprite = closeDoorSprite; // 扉の画像を閉じた状態に変更

            doorCollider.enabled = true; // 扉のコライダーを有効化
            if (doorCollider.enabled == false)
            {
            }
        }
    }
}
