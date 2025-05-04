using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public Button startButton;
    public Button restartButton;

    private bool Button = false;
    private bool playerDirection = true;
    private bool isGrounded;
    private bool isWall;
    private bool isSloop;
    private float playerWidth;
    private Vector3 move;
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startButton.onClick.AddListener(MoveButton);
        restartButton.onClick.AddListener(RestartButton);

        playerWidth = spriteRenderer.bounds.size.x;  // スプライトの幅を取得
        rb.freezeRotation = true; //
    }
        void Update()
    {
        if (Button)
        {
            HandleMovement();
        }
      
        HandleSpriteDirection();
    }

    void MoveButton()
    {
        Button = true;
    }

    void RestartButton()
    {
        Button = false;
    }
    // **移動処理**
    void HandleMovement()
    {
        Vector3 direction = playerDirection? Vector3.right : Vector3.left;

        // 前方に壁があるかチェック
        if (Physics.Raycast(transform.position, direction, 0.5f))
        {
          playerDirection = !playerDirection;  // 反転
        }


         isSloop= Physics.Raycast(transform.position, Vector3.down, 0.5f);







        //向きによって進む方向を変える
        if (playerDirection)
        {
            float moveX = +1.0f;
            move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
        else
        {
            float moveX = -1.0f;
            move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
 

    }

    // **ジャンプ処理**
    //void HandleJump()
    //{
    //    //着地判定
    //    isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.5f);


    //    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    //    {
    //        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    //    }

    //    if (!isGrounded && rb.velocity.y < 0)
    //    {
    //        rb.velocity += Vector3.up * Physics.gravity.y * 1.0f * Time.deltaTime;
    //    }
    //    if(!isGrounded && rb.velocity.y > 0)
    //    {
    //        rb.velocity += Vector3.up * Physics.gravity.y * 0.1f * Time.deltaTime;
    //    }
    //}

    // **プレイヤーの向き**
    void HandleSpriteDirection()
    {
      //無地の表示を変える
      if (playerDirection)
        {
            spriteRenderer.flipX = false; // 右向き
        }
        else
        {
            spriteRenderer.flipX = true; // 左向き
        }
    }
}


