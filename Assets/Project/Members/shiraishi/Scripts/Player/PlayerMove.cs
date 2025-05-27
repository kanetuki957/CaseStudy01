using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask wallLayer;  // 壁のレイヤーを指定

    private bool playerDirection = true;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector3 move;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Gameの状態がPlayingでないなら動かない
        if (GameManager.Instance.CurrentState != GameState.Playing)
        {
            return;
        }
        HandleMovement();

        HandleSpriteDirection();
    }

    // 移動
    void HandleMovement()
    {
        Vector2 direction = playerDirection ? Vector2.right : Vector2.left;

        // 2Dでの壁判定（レイキャスト）
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, wallLayer);
        if (hit.collider != null)
        {
            playerDirection = !playerDirection;  // 壁にぶつかったら向きを反転
        }

        float moveX = playerDirection ? 1.0f : -1.0f;
        move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }

    // プレイヤーの向きを変更
    void HandleSpriteDirection()
    {
        spriteRenderer.flipX = !playerDirection;
    }
}
