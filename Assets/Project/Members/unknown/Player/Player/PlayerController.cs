using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 5f;　//スピード
    public Button startButton;　　//スタートボタンの判定
    public Button restartButton; //リセットボタンの判定
    public Collider targetBlockCollider;//ターゲットの当たり判定
    
    private Vector3 startPostion;
    private bool Button = false;　　　　//ボタン判定
    private bool playerDirection = true;　//プレイヤーの向き
    private bool isGrounded;              //地面の設置判定
    private bool isWall;                  //壁の当たり判定
    private bool isGimmick;               //ギミックの判定
    private bool isTrap;                  //トラップの判定
    private bool isTrapReset;
    private bool isGoal;
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
        Vector3 direction = playerDirection? Vector3.right : Vector3.left;
        Vector3 rayOrigintop = transform.position + Vector3.up * 1f; // 頭の位置から発射
        Vector3 rayOrigindown = transform.position + Vector3.up * 1f; // 頭の位置から発射
        Vector3 rayOrigin1 = transform.position + Vector3.down * 0.5f; // 足元からレイを飛ばす

        // 前方に壁があるかチェック
        if (Physics.Raycast(rayOrigintop, direction, 0.2f))
        {
          playerDirection = !playerDirection;  // 反転
        }
        else if(Physics.Raycast(rayOrigindown, direction, 0.2f))
        {
            playerDirection = !playerDirection;  // 反転
        }

        RaycastHit hit;
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
            transform.position = startPostion + new Vector3(0,0.1f,0);  // 初期位置へ戻す
            playerDirection = true;// 右向き
        }
        isGoal = animationController.goalPerformance;
        if (isGoal)
        {
            direction = (target.position - rb.position).normalized;
             float desiredDistance = 2f; // 一定の距離を保つ
            float distance = Vector3.Distance(target.position, rb.position);
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
        if(collision.gameObject.name == "Goal")
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
}


