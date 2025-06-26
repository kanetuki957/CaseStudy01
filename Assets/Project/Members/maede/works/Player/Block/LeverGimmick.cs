using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverGimmick : MonoBehaviour
{
    public GameObject player;
    public GameObject followCharactor;
    private bool prevGimmick = false;
    private SpriteRenderer mySpriteRenderer;
    private AnimationController animationController;

    private bool isWaiting = false;
    private float waitStartTime = 0f;
    public float waitDuration = 0.5f; // ← 待ち時間（秒）
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        animationController = player.GetComponent<AnimationController>();
    }

    void Update()
    {
        bool isAnimating = animationController.gimmick;

        // アニメーション中は followCharactor を非表示
        if (isAnimating)
        {
            followCharactor.SetActive(false);
        }

        // アニメ終了した瞬間 → 待機開始
        if (prevGimmick && !isAnimating && !isWaiting)
        {
            isWaiting = true;
            waitStartTime = Time.time;
        }

        // 待機中 → 経過時間を見て処理
        if (isWaiting)
        {
            if (Time.time - waitStartTime >= waitDuration)
            {
                // プレイヤーの向きを取得
                SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();
                bool isFlipped = playerRenderer.flipX;

                Vector3 offset = isFlipped ? new Vector3(1f, 0, 0) : new Vector3(-1f, 0, 0);
                followCharactor.transform.position = player.transform.position + offset;
                followCharactor.SetActive(true);

                isWaiting = false; // 待機終了
            }
        }

        // レバーの表示
        mySpriteRenderer.enabled = !isAnimating;

        prevGimmick = isAnimating; // 前の状態を記録
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.GetComponent<PlayerController>() != null) // PlayerController を持っているかチェック
        {
            
            animationController.GimmickBool();

           
            mySpriteRenderer.flipX = !mySpriteRenderer.flipX;

           
        }

    }
}