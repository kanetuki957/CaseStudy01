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
    public Collider targetBlockCollider;//ターゲットの当たり判定
    public float baseJumpForce =0f; // 最小ジャンプ力
    public float maxJumpForce = 1.0f; // 最大ジャンプ力
    public float rayDistance = 1.0f; // レイキャストの距離
    public bool tt = false;
    public float jumpMoveDamping = 0.1f; // ジャンプ時の横移動の減速


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
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private AnimationController animationController;
    private float moveX;                   //X軸のmove変数

    void Start()
    {
        startPostion = transform.position;  // 初期位置を記録
        animationController = GetComponent<AnimationController>();
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startButton.onClick.AddListener(MoveButton);
        restartButton.onClick.AddListener(RestartButton);

        //playerWidth = spriteRenderer.bounds.size.x;  // スプライトの幅を取得
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
        //プレイヤーの向きを取得
        Vector3 direction = playerDirection ? Vector3.right : Vector3.left;
        Vector3 rayOrigintop = transform.position + Vector3.up * 0.5f; // 頭の位置から発射
        Vector3 rayOrigindown = transform.position + Vector3.up * 0.5f; // 頭の位置から発射
        Vector3 rayOrigin1 = transform.position + Vector3.down * 0.5f; // 足元からレイを飛ばす

        // 前方に壁があるかチェック
        if (Physics.Raycast(rayOrigintop, direction, 0.3f))
        {
            playerDirection = !playerDirection;  // 反転
        }
        else if (Physics.Raycast(rayOrigindown, direction, 0.3f))
        {
            playerDirection = !playerDirection;  // 反転
        }

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin1, Vector3.down, out hit, 10f))
        { // 最大10mの範囲で判定
            float groundDistance = Mathf.Round(hit.distance * 10.0f);

            //Debug.Log("床までの距離: " + groundDistance);
            rb.velocity += Vector3.down * 2f * Time.deltaTime;
        }
        RaycastHit hitBlock;
        if (Physics.Raycast(transform.position, direction, out hitBlock, rayDistance))
        {
            // スクリプトを持っているか確認
            JumpBlock jumpblock = hitBlock.collider.GetComponent<JumpBlock>();
            if (jumpblock != null) // スクリプトを持っているブロックだけ反応
            {
                float blockHeight = hitBlock.collider.bounds.size.y;
                float jumpForce = Mathf.Clamp(blockHeight * 0.5f, baseJumpForce, maxJumpForce);

                Jump(jumpForce);
                
            }

          
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
        if (isGrounded)
        {
            modifiedMoveX *= jumpMoveDamping; // ジャンプ中なら横移動を減速
        }
        moveX = modifiedMoveX;



        isGoalPerformance = animationController.goalPerformance;
        if (isGoalPerformance)
        {
            direction = (target.position - rb.position).normalized;
            float desiredDistance = 2f; // 一定の距離を保つ
            float distance = Vector3.Distance(target.position, rb.position);
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
        if (collision.gameObject.name == "Goal")
        {
            Button = false;
        }
        
        //if (collision.gameObject.GetComponent<JumpBlock>() != null) // MyScriptがアタッチされているか判定
        //{
        //    float blockTop = collision.gameObject.transform.position.y + collision.gameObject.transform.localScale.y / 2; // ブロックの上の座標を取得
        //    transform.position = new Vector3(transform.position.x, blockTop, transform.position.z); // 位置をブロックの上に修正

        //}
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
        Vector3 velocity = rb.velocity;

        float horizontalDamping = 0.5f; // 0.5f = 速度を50%に減らす
        velocity.x *= horizontalDamping; // X軸方向の速度を抑える

        // 変更した速度を適用
        rb.velocity = velocity;


        rb.AddForce(Vector3.up * force, ForceMode.Impulse); // ジャンプ力を適用
        animationController.JumpBool();
    }
}