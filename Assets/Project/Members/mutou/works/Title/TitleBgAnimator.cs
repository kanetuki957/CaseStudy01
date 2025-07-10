using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBgAnimator : MonoBehaviour
{
    [Tooltip("アニメーションで使うスプライト（順番に並べる）")]
    public List<Sprite> frames = new List<Sprite>();

    [Tooltip("1フレームの表示時間（秒）")]
    public float frameDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private int currentIndex = 0;
    private float timer = 0f;

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
        if (frames.Count == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameDuration)
        {
            timer -= frameDuration;
            currentIndex = (currentIndex + 1) % frames.Count; // ループ再生！
            spriteRenderer.sprite = frames[currentIndex];
        }
    }
}