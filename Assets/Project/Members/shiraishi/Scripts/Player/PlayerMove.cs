using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Move,
    Stay,
    Clime,
    Jump,
}

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    private int moveDirection = 1; // 初期値: 右向き

    // プロパティ（値を強制的に1か-1に）
    public int MoveDirection
    {
        get => moveDirection;
        set => moveDirection = (value >= 0) ? 1 : -1;
    }
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public PlayerState playerState = PlayerState.Stay; // 主にアニメーション用

    public LayerMask hitLayers; // 指定したレイヤーとぶつかる

    private float rayLength = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
        GameManager.Instance.OnGameStateChanged += HandleGameStateChange;
    }

    void Update()
    {
        // Player動作(物理演算以外)
        switch (playerState)
        {
            case PlayerState.Move:

                CheckCollisionAndFlipDirection();

                break;

            case PlayerState.Stay:

                break;

            case PlayerState.Clime:

                break;

            case PlayerState.Jump:

                break;
        }
    }

    private void FixedUpdate()
    {
        // Gameの状態がPlayingでないなら動かない
        if (GameManager.Instance.currentState != GameState.Playing)
        {
            return;
        }

        // Player動作(物理演算のみ)
        switch (playerState)
        {
            case PlayerState.Move:

                // 移動
                rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);

                break;

            case PlayerState.Stay:
                rb.velocity = new Vector2(0.0f, rb.velocity.y);
                break;

            case PlayerState.Clime:

                break;

            case PlayerState.Jump:

                break;
        }

    }

    // レイを飛ばしてぶつかったら反転
    void CheckCollisionAndFlipDirection()
    {
        float offset = GetComponent<CapsuleCollider2D>().size.x / 4;
        Vector2 origin = (Vector2)transform.position + new Vector2(moveDirection * offset, 0.0f);

        // 指定のレイヤーのみぶつかる
        int layerMask = hitLayers.value;

        RaycastHit2D hit = Physics2D.Raycast(origin, new Vector2(moveDirection, 0.0f), rayLength, layerMask);
        if (hit.collider != null)
        {
            moveDirection = -moveDirection;  // 壁にぶつかったら向きを反転
            Debug.Log("Hit:" + hit.collider.name);
            Debug.Log("moveDirection:" + new Vector2(moveDirection, 0.0f));

            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        // デバッグ可視化
        Debug.DrawRay(origin, new Vector2(moveDirection * rayLength, 0.0f), Color.red);
    }

    // ゲームステートの変更を検知して処理を実行
    void HandleGameStateChange(GameState prev, GameState curr)
    {
        if (curr == GameState.Playing && prev != GameState.Playing)
        {
            playerState = PlayerState.Move;
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= HandleGameStateChange;
    }
}

