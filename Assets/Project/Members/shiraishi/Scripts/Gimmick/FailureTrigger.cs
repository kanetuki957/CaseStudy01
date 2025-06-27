// ファイル名: FailureTrigger.cs

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FailureTrigger : MonoBehaviour
{
    [Tooltip("ゲームオーバー時に遷移するシーン（未指定の場合は現在のシーンを再読み込み）")]
    public string gameOverScene;

    [SerializeField] private EffectManager effectManager;

    //追記　前出 --------------------------

    [Header("トラップに当たった時のSE")]
    [SerializeField] private AudioClip trapHitSE;        // 壁に初ヒットした時の SE
    [SerializeField][Range(0f, 1f)] private float seVolume = 1f;

    private bool hasTriggered = false;
    public GameObject obj;
    private AnimationController controller;
    public float delay = 0f;

    //-----------------------------------------


    private void Start()
    {
        // 子から EffectManager を自動で探す（未設定時のみ）
        if (effectManager == null)
        {
            effectManager = GetComponentInChildren<EffectManager>();
            if (effectManager == null)
            {
                Debug.LogWarning($"[DisappearBlock] 子オブジェクトに EffectManager が見つかりません: {gameObject.name}");
            }
        }

       controller = obj.GetComponent<AnimationController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
 
        if (trapHitSE != null)
        {
            controller.TrapBool();
            AudioSource.PlayClipAtPoint(trapHitSE, Camera.main.transform.position, seVolume);
            //delay = trapHitSE.length;     // クリップ尺ぶん待つ
        }
        
        // ───── コルーチンをローカル関数で宣言 → その場で実行 ─────
        IEnumerator WaitAndLoad()
        {
            yield return new WaitForSecondsRealtime(delay);

            string scene = string.IsNullOrEmpty(gameOverScene)
                         ? SceneManager.GetActiveScene().name
                         : gameOverScene;

            SceneManager.LoadScene(scene);
        }
        StartCoroutine(WaitAndLoad());
    }




    //if (other.CompareTag("Player"))
    //{
    //    Debug.Log("[FailureTrigger] プレイヤーがトリガーに触れました");

    //    if (!string.IsNullOrEmpty(gameOverScene))
    //    {
    //        SceneManager.LoadScene(gameOverScene);
    //    }
    //    else
    //    {
    //        // シーンをリロード（リトライ用）
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //    }
    //}
}

