using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoAnimator : MonoBehaviour
{
    [Tooltip("アニメーションで使うスプライト（順番に並べる）")]
    public List<Sprite> frames = new List<Sprite>();

    [Tooltip("1フレームの表示時間（秒）")]
    public float frameDuration = 0.05f;

    private SpriteRenderer spriteRenderer;
    private int currentIndex = 0;
    private float timer = 0f;
    private bool isFinished = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (frames.Count == 0 || spriteRenderer == null)
        {
            enabled = false;
            return;
        }

        spriteRenderer.sprite = frames[0]; // 最初のスプライト表示
    }

    void Update()
    {
        if (isFinished || frames.Count == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameDuration)
        {
            timer -= frameDuration;
            currentIndex++;

            if (currentIndex < frames.Count)
            {
                spriteRenderer.sprite = frames[currentIndex];
            }
            else
            {
                isFinished = true; // アニメーション終了
            }
        }
    }
}