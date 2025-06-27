using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour, IActivatable
{
    [SerializeField]
    private Collider2D colliderOpen;  // 開いた状態の当たり判定
    [SerializeField]
    private Collider2D colliderClose; // 閉じた状態の当たり判定

    [SerializeField]
    private Sprite openSprite;  // 開いた橋のスプライト
    [SerializeField]
    private Sprite closeSprite; // 閉じた橋のスプライト

    private SpriteRenderer spriteRenderer;

    private bool isOpen = false;

    [Header("初期状態")]
    [SerializeField]
    private bool isOpenAtStart = false;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetBridgeState(isOpenAtStart);
        colliderOpen.enabled = false;
    }

    public void Activate()
    {
        SetBridgeState(!isOpen);
    }

    private void SetBridgeState(bool open)
    {
        isOpen = open;

        // コライダー切り替え
        //colliderOpen.enabled = isOpen;
        colliderClose.enabled = !isOpen;

        // スプライト切り替え
        spriteRenderer.sprite = isOpen ? openSprite : closeSprite;
    }
}
