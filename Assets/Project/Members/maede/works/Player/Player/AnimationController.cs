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
    public Sprite[] jumpFrames;      //ジャンプアニメーション
    public Sprite[] ladderFrames;    //はしごを上るアニメーション
    public Sprite[] getItemFrames;   //獲得アニメーション

    public float frameRate = 0.2f; 　// フレームの切り替え速度
    public float onlyFrameRate = 0.3f;
    public float jumpFrameRate = 0.3f;

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
    public bool jumpAnimation = false;
    public bool getItem = false;
    public bool ladder = false;
    public bool get = false;

    public float rayLength = 0f; // レイの長さ
    public int currentFrame;    //描写するフレーム
    public int currentOnlyFrame;
    public int currentJumpFrame;

    public bool Button = false;     //スタートボタンの判定
    private PlayerLadder2 playerLadder;
    private SpriteRenderer spriteRenderer;　//
    private Rigidbody2D rb;
    private float timer;             //時間
    private float Goaltimer = 0;     //ゴールアニメションの表示時間
    public  bool groundCheck = false;
    public bool skyLeapBool = false;
    private PlayerMove playerMove;

    [SerializeField] float rayLength1      = 0.2f;   // 射程
    [SerializeField] float stairMaxAngle  = 40f;    // 0?40° は階段扱い

    public bool isSupported { get; private set; }   // ← 地面 or 階段なら true



 

    void Start()
    {
        
        playerLadder = GetComponent<PlayerLadder2>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMove = GetComponent<PlayerMove>();
        startButton.onClick.AddListener(MoveButton);
        resetButton.onClick.AddListener(RestartButton);
       
    }

    void Update()
    {
        if (!GoalButton)
        {
            if (Button)
            {
                ladder = playerLadder.isOnLadder;
                GroundCheck();
                if (jumpAnimation)
                {
                    JumpBool();
                  
                }
                if (ladder)
                {
                    groundCheck = true;
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
                    if (get)
                    {
                        
                        OnlyFrame(getItemFrames);
                        if(framesAnimation)
                        {
                            currentOnlyFrame = 0;
                            framesAnimation= false;
                            get = false;
                          
                        }
                    }
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

                    if (trap)
                    {
                        OnlyFrame(trapFrames);
                        if (framesAnimation)
                        {
                            currentOnlyFrame = 0;
                            //framesAnimation = false;
                            //trap = false;
                        }
                    }

                  
                    if (ladder)
                    {
                        Frame(ladderFrames);
                    }


                    if (!trap && !gimmick && !ladder  && !get)
                    {
                        playerMove.enabled = false;
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
        framesAnimation = false;

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
            if (currentOnlyFrame +1 == frames.Length)
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
        if(skyLeapBool)
        {
            currentJumpFrame = 0;
            skyLeapBool = false;
        }

        if (frames.Length != 0)
        {
            if (currentJumpFrame == frames.Length -1)
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

        // 足元の左右2点から Ray を撃つ
        Vector2 basePos = transform.position;
        Vector2 originR = basePos + Vector2.down + Vector2.right * 0.5f;
        Vector2 originL = basePos + Vector2.down + Vector2.left * 0.5f;

        // Raycast ?? 左右どちらか当たればOK
        RaycastHit2D hitR = Physics2D.Raycast(originR, Vector2.down, rayLength1);
        RaycastHit2D hitL = Physics2D.Raycast(originL, Vector2.down, rayLength1);
        RaycastHit2D hitUnder =  Physics2D.Raycast(basePos, Vector2.down, rayLength1);

        // デバッグ可視化（Sceneビュー）
        Debug.DrawLine(originR, originR + Vector2.down * rayLength, Color.red);
        Debug.DrawLine(originL, originL + Vector2.down * rayLength, Color.red);

        // どちらか当たった側を使う
        RaycastHit2D hit = hitR ? hitR : hitL;
        if (hit.collider == null)
        {
            groundCheck = false;
            return;
        }
        
        if (hitUnder.collider.GetComponent<CheckLadder>() != null)
        {
            groundCheck = true;
            
        }
        else
        {
            if (!hit)
            {
                groundCheck = false;                      // 空中
                return;
            }

        }
          
       

        // 法線角で「壁」を除外
        float angle = Vector2.Angle(hit.normal, Vector2.up);
        groundCheck = angle <= stairMaxAngle;         // 地面 or 階段 ⇒ true

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