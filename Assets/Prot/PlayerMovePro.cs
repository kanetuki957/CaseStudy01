using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovePro : MonoBehaviour
{
   public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public Button startButton;
    public LayerMask wallLayer;  // 壁のレイヤーを指定

    private bool Button = false;
    private bool playerDirection = true;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private Vector3 move;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startButton.onClick.AddListener(MoveButton);
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (Button)
        {
            HandleMovement();
            HandleJump();
        }

        HandleSpriteDirection();
    }

    void MoveButton()
    {
        Button = true;
    }

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

    // 2Dジャンプ処理
    void HandleJump()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, wallLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 1.0f * Time.deltaTime;
        }
        if (!isGrounded && rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 0.1f * Time.deltaTime;
        }
    }

    void HandleSpriteDirection()
    {
        spriteRenderer.flipX = !playerDirection;
    }
}
