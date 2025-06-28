using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorAnimation : MonoBehaviour
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
    public Sprite[] jumpFrames;      //ジャンプアニメーション
    public Sprite[] ladderFrames;    //はしごを上るアニメーション
    public Sprite[] getItemFrames;   //獲得アニメーション

    public GameObject player;

    // スクリプトの参照
    private AnimationController scriptAni;

    public float frameRate = 0.2f; 　// フレームの切り替え速度
    public float onlyFrameRate = 0.3f;

    public Button startButton;　　 　//ボタンの判定
    public Button resetButton;       //ボタンの判定

    public bool GoalButton = false;　//ゴール判定
    public bool goalPerformance = false;      //ゴールアニメーション判定
    public bool gimmick = false;　　//ギミックの判定
    public bool trap = false;   //トラップのギミック
    public bool jump = false;
    public bool trapReset = false;
    public bool finish = false;
    public bool framesAnimation = false;
    public bool jumpAnimation = false;
    public bool getItem = false;
    public bool ladder = false;
    public bool get = false;

    public float rayLength = 0f; // レイの長さ
    public int currentFrame;    //描写するフレーム
    public int currentOnlyFrame;
    public int currentJumpFrame;

    public bool Button = false;     //スタートボタンの判定
    private SpriteRenderer spriteRenderer;　//
    private Rigidbody2D rb;
    private float timer;             //時間
    private float Goaltimer = 0;     //ゴールアニメションの表示時間
    public bool groundCheck = false;
    public bool skyLeapBool = false;
    private PlayerMove playerMove;




    void Start()
    {
        scriptAni = player.GetComponent<AnimationController>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startButton.onClick.AddListener(MoveButton);
        resetButton.onClick.AddListener(RestartButton);

    }

    void Update()
    {
        GoalButton = scriptAni.GoalButton;
        if (!GoalButton)
        {
            Button = scriptAni.Button;
            if (Button)
            {
                jump = scriptAni.jump;


                GroundCheck();
                if (jumpAnimation)
                {
                    JumpBool();
                }

                if (!groundCheck)
                {
                    SkyLeap();
                    if (jump)
                    {
                        JumpFrame(jumpFrames);
                        if (framesAnimation)
                        {
                            framesAnimation = false;

                        }
                    }
                    else
                    {
                        Frame(fallFrames);
                        skyLeapBool = true;

                    }
                }
                else
                {
                    get = scriptAni.get;
                    if (get)
                    {

                        OnlyFrame(getItemFrames);
                        if (framesAnimation)
                        {
                            currentOnlyFrame = 0;
                            framesAnimation = false;
                            get = false;

                        }
                    }

                    gimmick = scriptAni.gimmick;
                    if (gimmick)
                    {

                        OnlyFrame(gimmickFrames);
                        if (framesAnimation)
                        {
                            currentOnlyFrame = 0;
                            gimmick = false;
                            framesAnimation = false;
                        }
                    }
                    trap = scriptAni.trap;

                    if (trap)
                    {
                        OnlyFrame(trapFrames);
                        if (framesAnimation) // 1秒後にfalseにする
                        {
                            currentOnlyFrame = 0;
                            trapReset = true;
                            trap = false;
                            spriteRenderer.sprite = idleFrames[0];
                            Button = false;
                            framesAnimation = false;
                        }
                    }
                    if (!trap && !gimmick && !get)
                    {
                       
                        Frame(moveFrames);
                        skyLeapBool = true;
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
            //goalPerformance = scriptAni.goalPerformance;

            //if (!goalPerformance)
            //{
            //    OnlyFrame(goalFrames);
            //    if (framesAnimation) // 2秒後にfalseにする
            //    {
            //        currentOnlyFrame = 0;
            //        goalPerformance = true;
            //        framesAnimation = false;
            //    }
            //}
            //else
            //{
            //    Goaltimer += Time.deltaTime;
            //    Frame(goalPerformanceFrame);
            //    if (Goaltimer > 5)
            //    {

            //        goalPerformance = false;
            //        Goaltimer = 0;
            //        spriteRenderer.sprite = idleFrames[0];
            //        Button = false;
            //        GoalButton = false;
            //        finish = true;

            //    }
            //}
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
        groundCheck = false;
        jump = true;
    }
    public void getItemBool()
    {

    }

    //アニメーション関数
    void Frame(Sprite[] frames)
    {
        if (frames.Length != 0)
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
            if (currentOnlyFrame + 1 == frames.Length)
            {
                framesAnimation = true;
                currentOnlyFrame = 0;
                return;
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= onlyFrameRate)
                {
                    timer = 0f;
                    currentOnlyFrame++;
                    if (currentOnlyFrame >= frames.Length)
                        currentOnlyFrame = frames.Length - 1;
                    spriteRenderer.sprite = frames[currentOnlyFrame];
                }
            }
        }
    }

    void JumpFrame(Sprite[] frames)
    {
        if (skyLeapBool)
        {
            currentJumpFrame = 0;
            skyLeapBool = false;
        }

        if (frames.Length != 0)
        {
            if (currentJumpFrame == frames.Length - 1)
            {
                framesAnimation = true;

                return;
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= onlyFrameRate)
                {
                    timer = 0f;
                    currentJumpFrame++;
                    if (currentJumpFrame >= frames.Length)
                        currentJumpFrame = frames.Length - 1;
                    spriteRenderer.sprite = frames[currentJumpFrame];
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PickupableItem>())
        {
            get = true;

        }
    }

    
}