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
    public float frameRate = 0.2f; 　// フレームの切り替え速度
    public Button startButton;　　 　//ボタンの判定
    public Button resetButton;       //ボタンの判定
    private bool Button = false;     //スタートボタンの判定
    private bool GoalButton = false;　//ゴール判定
    public bool goalPerformance = false;      //ゴールアニメーション判定
    public bool gimmick = false;　　//ギミックの判定
    public bool trap = false;       //トラップのギミック
    public bool trapReset= false;
    private SpriteRenderer spriteRenderer;　//
    private int currentFrame;　　　　//描写するフレーム
    private float timer;             //時間
    private float Goaltimer = 0;     //ゴールアニメションの表示時間




    void Start()
    {
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
            
                if (gimmick)
                {
                    Goaltimer += Time.deltaTime;
                    Frame(gimmickFrames);
                    if (Goaltimer >= 1f) // 1秒後にfalseにする
                    {
                        gimmick = false;
                        Goaltimer = 0;
                    }
                }
                
                if (trap)
                {
                    Goaltimer += Time.deltaTime;
                    Frame(trapFrames);
                    if (Goaltimer >= 1f) // 1秒後にfalseにする
                    {
                        trapReset = true;
                        trap = false;
                        Goaltimer = 0;
                        spriteRenderer.sprite = idleFrames[0];
                        Button = false;
                    }
                }
                if(!trap&& !gimmick)
                {
                    Frame(moveFrames);
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
  
                Goaltimer += Time.deltaTime;
                Frame(goalFrames);
                if (Goaltimer >= 2f) // 2秒後にfalseにする
                {
                    goalPerformance = true;
                    Goaltimer = 0;
                }
            }
            else
            {
                Goaltimer += Time.deltaTime;
                Frame(goalPerformanceFrame);
                if (Goaltimer >= 3f) // 3秒後にfalseにする
                {
                    goalPerformance = false;
                    Goaltimer = 0;
                    spriteRenderer.sprite = idleFrames[0];
                    Button = false;
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
}