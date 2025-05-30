using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 4f; //スピード
    public float jumpForce = 8f;  // ジャンプ力
    public bool Button = false;　　　　//ボタン判定
    public Button startButton;　　//スタートボタンの判定
    public Button restartButton; //リセットボタンの判定
    public Collider2D targetBlockCollider;//ターゲットの当たり判定
    private Collider2D myCollider;
    public GameObject[] ignoredPlayers; // 通り抜けたいプレイヤーのリスト
    public float baseJumpForce =0f; // 最小ジャンプ力
    public float maxJumpForce = 1.0f; // 最大ジャンプ力
    public float rayDistance = 1.0f; // レイキャストの距離
    public int   jumpBlock = 1;　//ジャンプできるブロックの高さ設定
    public float jumpMoveDamping = 0.1f; // ジャンプ時の横移動の減速
    public float wallsRay = 0.0f;
    private Vector3 startPostion;
    private bool playerDirection = true;　//プレイヤーの向き
    public bool isGrounded;              //地面の設置判定
    private bool isWall;                  //壁の当たり判定
    private bool isGimmick;               //ギミックの判定
    private bool isTrap;                  //トラップの判定
    private bool isTrapReset;
    private bool isGoal;
    private bool isGoalPerformance;
    private bool isFinish;
    private Vector3 move;                 //move変数
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AnimationController animationController;
    private float moveX;                   //X軸のmove変数

    void Start()
    {
        startPostion = transform.position;  // 初期位置を記録
        animationController = GetComponent<AnimationController>();
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startButton.onClick.AddListener(MoveButton);
        restartButton.onClick.AddListener(RestartButton);

        //playerWidth = spriteRenderer.bounds.size.x;  // スプライトの幅を取得
        rb.freezeRotation = true; //
        for (int i = 0; i < ignoredPlayers.Length; i++)
        {
            Collider2D playerCollider = ignoredPlayers[i].GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, playerCollider);
            }
        }
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
        //プレイヤーの向きを取得
        Vector2 direction = playerDirection ? Vector2.right : Vector2.left;
        Vector2 rayOrigin = (Vector2)transform.position + direction * 0.4f; // 頭の位置から発射

        // 前方に壁があるかチェック
        RaycastHit2D hitwalls = Physics2D.Raycast(rayOrigin, direction, wallsRay);
        if (hitwalls.collider != null)
        {
            Debug.Log("真ん中" + hitwalls.collider.name);
            playerDirection = !playerDirection;  // 反転
        }
        //RaycastHit2D hit = Physics2D.Raycast(rayOrigindown, Vector2.down, 10f);
        //if (hit.collider != null)
        // { // 最大10mの範囲で判定
        //    float groundDistance = Mathf.Round(hit.distance * 10.0f);

        //    //Debug.Log("床までの距離: " + groundDistance);
        //    rb.velocity += Vector2.down * 2f * Time.deltaTime;
        //}
         RaycastHit2D hitBlock = Physics2D.Raycast(rayOrigin, direction, rayDistance);
        if (hitBlock.collider != null)
        {
            float blockHeight = hitBlock.collider.bounds.size.y;
            if (blockHeight < jumpBlock) 
            {

                float jumpForce = Mathf.Clamp(blockHeight * 0.1f, baseJumpForce, maxJumpForce);

                Jump(jumpForce);

            }         
        }

        //向きによって進む方向を変える
        if (playerDirection)
        {
            moveX = 1.0f;
        }
        else
        {
            moveX = -1.0f;
        }
        isGrounded = animationController.groundCheck;
        //ギミック時停止
        isGimmick = animationController.gimmick;
        StopCharactor(isGimmick);
        //トラップ時停止
        isTrap = animationController.trap;
        StopCharactor(isTrap);
        //トラップにかかり初期位置に戻る
        isTrapReset = animationController.trapReset;
        if (isTrapReset)
        {
            transform.position = startPostion + new Vector3(0, 0.05f, 0);  // 初期位置へ戻す
            playerDirection = true;// 右向き
        }
        isGoal = animationController.GoalButton;
        if (isGoal)
        {
            StopCharactor(isGoal);

        }

        float modifiedMoveX = moveX;

        // 地面にいるときは通常の移動
        if (!isGrounded)
        {
            modifiedMoveX *= jumpMoveDamping; // ジャンプ中なら横移動を減速
        }
        moveX = modifiedMoveX;



        isGoalPerformance = animationController.goalPerformance;
        if (isGoalPerformance)
        {
            direction = (target.position - transform.position).normalized;
            float desiredDistance = 2f; // 一定の距離を保つ
            float distance = Vector2.Distance(target.position, rb.position);
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // 右向き
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true; // 左向き
            }
            // 一定の距離より近い場合は移動しない
            if (distance > desiredDistance)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
        isFinish = animationController.finish;

        if (isFinish)
        {
            Button = false;
        }

    }

    void HandleSpriteDirection()
    {
        spriteRenderer.flipX = !playerDirection;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == targetBlockCollider)
        {
            enabled = false;  // スクリプトの動作を停止
        }
        if (collision.gameObject.name == "Goal")
        {
            Button = false;
        }
    }
    private void StopCharactor(bool isbool)
    {
        if (isbool)
        {
            moveX = 0.0f;
        }
    }
    
    void Jump(float force)
    {
        // 現在の横移動速度を取得
        Vector2 velocity = rb.velocity;

        float horizontalDamping = 0.5f; // 0.5f = 速度を50%に減らす
        velocity.x *= horizontalDamping; // X軸方向の速度を抑える

        // 変更した速度を適用
        rb.velocity = velocity;


        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse); // ジャンプ力を適用
        animationController.JumpBool();
    }


}