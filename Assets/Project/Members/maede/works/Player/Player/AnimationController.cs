using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    // スプライトの配列 
    public Sprite[] idleFrames;      //停止時アニメーション
    public Sprite[] moveFrames;      //動作時アニメーション　
    public Sprite[] goalFrames; 　　 //ゴール時アニメーション
    public Sprite[] goalPerformanceFrame;//ゴール後のアニメーション　
    public Sprite[] gimmickFrames;   //ギミック時アニメーション　
    public Sprite[] trapFrames; 　　 //トラップ時アニメーション　
    public Sprite[] getupFrames;     //目が覚めるアニメーション　
    public Sprite[] fallFrames;      //落ちるアニメーション
    public Sprite[] jumpFrames;

    public float frameRate = 0.2f; 　// フレームの切り替え速度

    public Button startButton;　　 　//ボタンの判定
    public Button resetButton;       //ボタンの判定

    public bool GoalButton = false;　//ゴール判定
    public bool goalPerformance = false;      //ゴールアニメーション判定
    public bool gimmick = false;　　//ギミックの判定
    public bool trap = false;   //トラップのギミック
    public bool jump = false;
    public bool trapReset= false;
    public bool finish = false;
    public bool framesAnimation = false;

    public float rayLength = 0f; // レイの長さ
    public int currentFrame;    //描写するフレーム

    private bool Button = false;     //スタートボタンの判定
    private SpriteRenderer spriteRenderer;　//
    private Rigidbody2D rb;
    private float timer;             //時間
    private float Goaltimer = 0;     //ゴールアニメションの表示時間
    public  bool groundCheck = false;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startButton.onClick.AddListener(MoveButton);
        resetButton.onClick.AddListener(RestartButton);
       
    }

    void Update()
    {
        if (!GoalButton)
        {
            if (Button)
            {
              
                GroundCheck();
                if (!groundCheck)
                {
                    SkyLeap();
                    if (jump)
                    {
                        
                        OnlyFrame(jumpFrames);
                    }
                    else
                    {
                        Frame(fallFrames);
                    }
                }
                else
                {
                    if (gimmick)
                    {

                        OnlyFrame(gimmickFrames);
                        if (framesAnimation)
                        {
                            gimmick = false;
                            framesAnimation = false;
                        }
                    }

                    if (trap)
                    {
                        OnlyFrame(trapFrames);
                        if (framesAnimation) // 1秒後にfalseにする
                        {
                            trapReset = true;
                            trap = false;
                            spriteRenderer.sprite = idleFrames[0];
                            Button = false;
                            framesAnimation = false;
                        }
                    }
                    if (!trap && !gimmick)
                    {
                        Frame(moveFrames);
                    }
                }
            }
            else
            {
                Frame(idleFrames);
            }
        }
        else
        {
            if (!goalPerformance)
            {
                OnlyFrame(goalFrames);
                if (framesAnimation) // 2秒後にfalseにする
                {
                    goalPerformance = true;
                    framesAnimation = false;
                }
            }
            else
            {
                Goaltimer += Time.deltaTime;
                Frame(goalPerformanceFrame);
                if (Goaltimer > 5) 
                {

                    goalPerformance = false;
                    Goaltimer = 0;
                    spriteRenderer.sprite = idleFrames[0];
                    Button = false;
                    GoalButton = false;
                    finish = true;

                }
            }
        }
    }
    //スタートボタンの処理
    void MoveButton()
    {
        Goaltimer = 0;
        goalPerformance = false;
        Button = true;
        trapReset = false;
        finish = false;
    }
    //リセットボタンの処理
    void RestartButton()
    {
        spriteRenderer.sprite = idleFrames[0];
        Button = false;
    }

    //ギミックの当たり判定
    public void GimmickBool()
    {
        gimmick = true; 
    }
    public void TrapBool()
    {
        trap = true;
    }
    public void GoalBool()
    {
        GoalButton = true;
    }
    public void JumpBool()
    {
        jump = true;
    }
    //アニメーション関数
    void Frame(Sprite[] frames)
    {
        if(frames.Length != 0)
        {
            timer += Time.deltaTime;
            if (timer >= frameRate)
            {
                timer = 0f;
                currentFrame = (currentFrame + 1) % frames.Length;
                spriteRenderer.sprite = frames[currentFrame];
            }
        }
    }
    void OnlyFrame(Sprite[] frames)
    {
        if (frames.Length != 0)
        {

            if (currentFrame +1 == frames.Length)
            {
                framesAnimation = true;
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= frameRate)
                {
                    timer = 0f;
                    currentFrame = (currentFrame + 1) % frames.Length;
                    spriteRenderer.sprite = frames[currentFrame];
                }
            }           
        }
    }
    void GroundCheck()
    {
       Vector2 rayOrigin = (Vector2)transform.position + Vector2.down; // 頭の位置から発射
       

       RaycastHit2D hitGround = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength);
        if (hitGround.collider != null)
        {
       
            groundCheck = true;
        }
        else
        {
            groundCheck = false;
        }

      
    }
    void SkyLeap()
    {
        if (rb.velocity.y > 0) // 正の速度ならジャンプ中
        {
            jump = true;
        }
        else if (rb.velocity.y < 0) // 負の速度なら落下中
        {
            jump = false;
        }
   
    }
}