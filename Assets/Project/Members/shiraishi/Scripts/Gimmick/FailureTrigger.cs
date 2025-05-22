// ファイル名: FailureTrigger.cs

using UnityEngine;
using UnityEngine.SceneManagement;

public class FailureTrigger : MonoBehaviour
{
    [Tooltip("ゲームオーバー時に遷移するシーン（未指定の場合は現在のシーンを再読み込み）")]
    public string gameOverScene;

    [SerializeField] private EffectManager effectManager;

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[FailureTrigger] プレイヤーがトリガーに触れました");

            if (!string.IsNullOrEmpty(gameOverScene))
            {
                SceneManager.LoadScene(gameOverScene);
            }
            else
            {
                // シーンをリロード（リトライ用）
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
