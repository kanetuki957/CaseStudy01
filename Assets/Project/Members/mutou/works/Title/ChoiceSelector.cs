using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// タイトル画面で選択肢（Choice）を操作するクラス
public class ChoiceSelector : MonoBehaviour
{
    // クリック音（選択移動・決定に使用）
    public AudioClip clickSound;

    // 効果音再生用
    private AudioSource audioSource;

    // 上下の選択位置
    private Vector3 upperPosition;
    private Vector3 lowerPosition;

    // 現在の選択位置が上かどうか（true = upper）
    private bool isUpper = true;

    void Start()
    {
        // 初期位置を設定（現在のX,Zを基にYを決定）
        float x = transform.position.x;
        float z = transform.position.z;

        upperPosition = new Vector3(x, -1.1f, z); // ゲームスタート位置
        lowerPosition = new Vector3(x, -3.4f, z); // ゲーム終了位置

        // 初期位置を上に設定
        transform.position = upperPosition;

        // AudioSourceを生成し初期設定
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        audioSource.volume = 1.0f; // 音量を最大に設定
    }

    void Update()
    {
        // ↑キー：上の選択肢へ移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayClickSound();
            MoveToUpper();
        }

        // ↓キー：下の選択肢へ移動
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayClickSound();
            MoveToLower();
        }

        // Enterキー：現在の選択に応じて処理実行
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayClickSound();

            if (isUpper)
            {
                // 「ゲームスタート」選択時：フェード付きでシーン遷移
                FadeManager.Instance.LoadScene("SelectScene", 1f);
            }

            else
            {
                // 「おわり」選択時：アプリケーション終了
                PlayClickAndQuit();
            }
        }
    }

    // 上の選択肢に移動（Y座標を変更）
    void MoveToUpper()
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, -1.1f, pos.z);
        isUpper = true;
    }

    // 下の選択肢に移動（Y座標を変更）
    void MoveToLower()
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, -3.4f, pos.z);
        isUpper = false;
    }

    // クリック音を再生
    void PlayClickSound()
    {
        if (clickSound != null) audioSource.PlayOneShot(clickSound);
    }

    // アプリケーション終了
    void PlayClickAndQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}