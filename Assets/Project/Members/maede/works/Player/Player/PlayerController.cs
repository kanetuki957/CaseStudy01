using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject targetObject;
    public Transform target;
    public float moveSpeed = 4f; //スピード
    public bool Button = false;　　　　//ボタン判定
    public Button startButton;　　//スタートボタンの判定
    public Button restartButton; //リセットボタンの判定
    public Collider2D targetBlockCollider;//ターゲットの当たり判定
    private Collider2D myCollider;
    public GameObject[] ignoredPlayers; // 通り抜けたいプレイヤーのリスト
    public float JumpForce = 1.0f; //ジャンプ力
    public float rayDistance = 1.0f; // レイキャストの距離
    public int jumpBlock = 1;　//ジャンプできるブロックの高さ設定
    public float jumpMoveDamping = 0.1f; // ジャンプ時の横移動の減速
    public float wallsRay = 0.0f;
    public float jumpWait = 0.1f;
    private float moveX;                   //X軸のmove変数
    private float Goaltimer = 0;     //ゴールアニメションの表示時間

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
    public bool isjump;
    public bool isget;
    public bool isladder = false;
    private bool i = false;
    private Vector3 move;                 //move変
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AnimationController animationController;

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

        //向きによって進む方向を変える
        if (playerDirection)
        {
            moveX = 1.0f;
        }
        else
        {
            moveX = -1.0f;
        }
        //プレイヤーの向きを取得
        Vector2 direction = playerDirection ? Vector2.right : Vector2.left;
        Vector2 rayOrigin = (Vector2)transform.position + direction * 0.4f; // 頭の位置から発射

        isladder = animationController.ladder;

        if (isladder)
        {
            targetObject.SetActive(false);
            target.position = transform.position; // 初期位置へ戻す
        }
        else
        {
            targetObject.SetActive(true);
        }



        if (i && !isladder)
        {
            if (direction == Vector2.right)
            {
                transform.position = (Vector2)transform.position + direction * 0.5f;
            }
            else
            {
                transform.position = (Vector2)transform.position - direction * 0.5f;
            }
        }
        i = isladder;
        // 前方にものがあるかチェック
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, wallsRay);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.gameObject);
            if (hit.collider is BoxCollider2D && hit.collider.GetComponent<CheckLadder>() == null && hit.collider.GetComponent<LeverGimmick>() == null && hit.collider.GetComponent<PickupableItem>() == null && hit.collider.GetComponent<FailureTrigger>() == null)
            {
                if (hit.collider.gameObject.name != "followcharactor" && hit.collider.name != "Goal")
                {

                    // targetBlockCollider に当たったかチェック
                    if (hit.collider == targetBlockCollider)
                    {

                        hit.collider.enabled = false;  // スクリプト停止などの処理
                        return;
                    }

                    if (!animationController.ladder )
                    {
                        Debug.Log(hit.collider.name);
                        //Debug.Log("真ん中" + hitwalls.collider.name);
                        playerDirection = !playerDirection;  // 反転
                    }

                    //// 名前が Goal のオブジェクトに当たったか
                    //if (hit.collider.name == "Goal")
                    //{

                    //    animationController.GoalBool();
                    //    hit.collider.enabled = false; // 当たり判定をオフにする
                    //    return;
                    //}
                    //else
                    //{
                    //    if (!animationController.ladder)
                    //    {
                    //        //Debug.Log("真ん中" + hitwalls.collider.name);
                    //        playerDirection = !playerDirection;  // 反転
                    //    }                            //}
                    //}
                }
            }
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
            if (hitBlock.collider.GetComponent<JumpBlock>() != null)
            {
                float blockHeight = hitBlock.collider.bounds.size.y;
                if (blockHeight < jumpBlock)
                {

                    Goaltimer += Time.deltaTime;
                    if (Goaltimer < jumpWait)
                    {

                        StopCharactor(Button);
                        animationController.jumpAnimation = true;
                    }
                    else
                    {
                        Jump(JumpForce);
                    }
                }
            }
        }
        else
        {
            animationController.jumpAnimation = false;
        }

        isjump = animationController.skyLeapBool;
        if (isjump)
        {
            Goaltimer = 0;
        }

        isget = animationController.get;
        StopCharactor(isget);

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
            transform.position = (Vector2)startPostion + direction;  // 初期位置へ戻す
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
            if (distance > desiredDistance)
            {
                Vector2 targetVelocity = new Vector2(direction.x * moveSpeed, rb.velocity.y); // Yは今のまま（重力影響を維持）
                rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, 0.5f);
            }
            else
            {

                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0, rb.velocity.y), 0.5f);
            }
        }
        else
        {

            //rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
            move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
            if (moveX == 0)
            {
                rb.velocity = new Vector2(0, 0); // 横移動を完全に止める
            }
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


    private void StopCharactor(bool isbool)
    {
        if (isbool)
        {
            moveX = 0.0f;
            rb.velocity = new Vector2(0, rb.velocity.y);  // ? 横移動を完全に止める ← ここが追加された変更ポイント
        }
    }

    void Jump(float force)
    {

        // 現在の横移動速度を取得
        Vector2 velocity = rb.velocity;

        float horizontalDamping = 0.3f; // 0.5f = 速度を50%に減らす
        velocity.x *= horizontalDamping; // X軸方向の速度を抑える

        // 変更した速度を適用
        rb.velocity = velocity;


        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse); // ジャンプ力を適用
    }
}