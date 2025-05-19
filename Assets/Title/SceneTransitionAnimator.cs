using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionAnimator : MonoBehaviour
{
    [Tooltip("アニメーション対象の GameObject（UIじゃなくてもOK）")]
    public List<GameObject> targets = new List<GameObject>();

    public float duration = 0.6f; // アニメーションの長さ

    [Tooltip("スケールの変化カーブ")]
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    [Tooltip("Z軸回転の変化カーブ")]
    public AnimationCurve rotateCurve = AnimationCurve.EaseInOut(0, 0, 1, 360);

    // シーン遷移前に演出を挟む（完了時にonCompleteを呼び出す）
    public IEnumerator AnimateAndSwitch(System.Action onComplete)
    {
        float time = 0f;

        // 対象オブジェクトの初期スケールと回転を保存
        Dictionary<Transform, Vector3> startScales = new();
        Dictionary<Transform, Quaternion> startRotations = new();
        List<Transform> transforms = new();

        foreach (var go in targets)
        {
            if (go != null)
            {
                var tf = go.transform;
                transforms.Add(tf);
                startScales[tf] = tf.localScale;
                startRotations[tf] = tf.rotation;
            }
        }

        // アニメーション処理
        while (time < duration)
        {
            float tNorm = time / duration;
            float scaleVal = scaleCurve.Evaluate(tNorm);
            float rotZ = rotateCurve.Evaluate(tNorm);

            foreach (var tf in transforms)
            {
                tf.localScale = startScales[tf] * scaleVal;
                tf.rotation = Quaternion.Euler(0, 0, rotZ); // Z回転だけ
            }

            time += Time.deltaTime;
            yield return null;
        }

        // アニメーション完了時にコールバックを呼ぶ
        onComplete?.Invoke();
    }
}