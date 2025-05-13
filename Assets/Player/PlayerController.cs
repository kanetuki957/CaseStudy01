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
    public Collider targetBlockCollider;


    private bool Button = false;
    private bool playerDirection = true;
    private bool isGrounded;
    private bool isWall;
    private float playerWidth;
    private Vector3 move;
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private float moveX;

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
        enabled = true;  // スクリプトの動作を停止
        Button = false;
        playerDirection = true;
    }
    // **移動処理**
    void HandleMovement()
    {
        Vector3 direction = playerDirection? Vector3.right : Vector3.left;
        Vector3 rayOrigin = transform.position + Vector3.up * 1f; // 頭の位置から発射
        Vector3 rayOrigin1 = transform.position + Vector3.down * 0.5f; // 足元からレイを飛ばす

        // 前方に壁があるかチェック
        if (Physics.Raycast(rayOrigin, direction, 0.5f))
        {
          playerDirection = !playerDirection;  // 反転
        }

        RaycastHit hit;
        //if (Physics.Raycast(rayOrigin1, Vector3.down, out hit, 1f))
        //{

        //    float slopeAngle = Vector3.Angle(hit.normal, Vector3.up); // 地面の傾斜角を取得
        //    //Debug.Log("傾斜角: " + slopeAngle);
        //    //rb.velocity += Vector3.down * 2f * Time.deltaTime;  // 重力を強める

        //}
        if (Physics.Raycast(rayOrigin1, Vector3.down, out hit, 10f))
        { // 最大10mの範囲で判定
            float groundDistance = Mathf.Round(hit.distance* 10.0f);
           
            //Debug.Log("床までの距離: " + groundDistance);
            rb.velocity += Vector3.down * 2f * Time.deltaTime;
        }





      
        


        //向きによって進む方向を変える
       if (playerDirection)
       {
               moveX = +1.0f;
       }
       else
       {
            moveX = -1.0f;
       }
      
        move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);


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
            spriteRenderer.flipX = false; // 左向き
        }
        else
        {
            spriteRenderer.flipX = true; // 右向き
        }
    }
 
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == targetBlockCollider)
        {
            Debug.Log("Touched specific block!");
            enabled = false;  // スクリプトの動作を停止
        }
    }
}


