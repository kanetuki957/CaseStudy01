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
    public Sprite[] gimmickFrames;   //ギミック時アニメーション　
    public Sprite[] trapFrames; 　　 //トラップ時アニメーション　
    public Sprite[] getupFrames;     //目が覚めるアニメーション　
    public float frameRate = 0.1f; 　// フレームの切り替え速度
    public Button startButton;　　 　//ボタンの判定
    public Button resetButton;       //ボタンの判定
    private bool Button = false;     //スタートボタンの判定
    private bool GoalButton = true;　//ゴール判定
    private bool result = true;      //ゴールアニメーション判定
    private bool gimmick = false;　　//ギミックの判定
    private bool trap = false;       //トラップのギミック
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
        if (GoalButton)
        {
            if (Button)
            {
            
                if (gimmick)
                {
                    
                    frame(gimmickFrames);
                    if (gimmickFrames.Length == currentFrame) // 3秒後にfalseにする
                    {
                        gimmick = false;
                        
                    }
                }
                else if (trap)
                {
                   
                    frame(trapFrames);
                    if (trapFrames.Length == currentFrame) // 3秒後にfalseにする
                    {
                        trap = false;
                        
                    }
                }
                else
                {
                    frame(moveFrames);
                }
            }
            else
            {
                frame(idleFrames);
            }
        }
        else
        {
            if (result)
            {
  
                 Goaltimer += Time.deltaTime;
                frame(goalFrames);
                if (Goaltimer >= 3f) // 3秒後にfalseにする
                {                  
                    result = false;
                    Goaltimer = 0;
                }
            }
          
        }
    }
    //スタートボタンの処理
    void MoveButton()
    {
        Goaltimer = 0;
     result = true;
        Button = true;
    }
    //リセットボタンの処理
    void RestartButton()
    {
        spriteRenderer.sprite = idleFrames[0];
        Button = false;
       
    }
    //当たり判定
    void OnCollisionEnter(Collision collision)
    {
        //ゴールと触れたら
        if (collision.gameObject.name == "Goal")
        {

            GoalButton =  false;
        }
    }
    //アニメーション関数
    void frame(Sprite[] frames)
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