using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoAnimator : MonoBehaviour
{
    public float duration = 1.5f;     // ロゴが表示されるまでのアニメーション時間
    public AnimationCurve curve;      // 拡大に使う補間カーブ

    private Vector3 initialScale;     // 元のスケールを保存しておく
    private CanvasGroup canvasGroup;  // アルファ値制御用（フェードイン）

    void Awake()
    {
        // スケールを0にしてロゴを非表示状態にしておく
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;

        // 透明状態にするためのCanvasGroupを追加
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    void Start()
    {
        // アニメーション開始
        StartCoroutine(AnimateLogo());
    }

    IEnumerator AnimateLogo()
    {
        float time = 0f;

        // durationの時間でロゴをスケーリング・フェードインする
        while (time < duration)
        {
            float t = time / duration;
            float eased = curve.Evaluate(t);

            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, eased);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            time += Time.deltaTime;
            yield return null;
        }

        // 最終的に完全表示
        transform.localScale = initialScale;
        canvasGroup.alpha = 1f;
    }
}