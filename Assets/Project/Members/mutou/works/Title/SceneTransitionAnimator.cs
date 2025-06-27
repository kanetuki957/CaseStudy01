using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionAnimator : MonoBehaviour
{
    [Tooltip("アニメーション対象の GameObject（UIじゃなくてもOK）")]
    public List<GameObject> targets = new List<GameObject>();

    public float duration = 0.6f; // アニメーションの長さ

    [Tooltip("フェードアウトカーブ")]
    public AnimationCurve fadeOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    [Tooltip("フェードインカーブ（戻し用）")]
    public AnimationCurve fadeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    // シーン遷移前にフェードアウト演出を挟む（完了時にonCompleteを呼び出す）
    public IEnumerator AnimateAndSwitch(System.Action onComplete)
    {
        float time = 0f;

        // CanvasGroupによる透明度制御
        List<CanvasGroup> canvasGroups = new();

        foreach (var go in targets)
        {
            if (go != null)
            {
                var cg = go.GetComponent<CanvasGroup>();
                if (cg == null)
                {
                    cg = go.AddComponent<CanvasGroup>();
                }
                canvasGroups.Add(cg);
            }
        }

        // フェードアウト
        while (time < duration)
        {
            float t = time / duration;
            float alphaVal = fadeOutCurve.Evaluate(t);

            foreach (var cg in canvasGroups)
            {
                cg.alpha = alphaVal;
            }

            time += Time.deltaTime;
            yield return null;
        }

        onComplete?.Invoke();
    }
}