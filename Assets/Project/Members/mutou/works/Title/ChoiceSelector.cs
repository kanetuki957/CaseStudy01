// タイトル画面で上下の選択肢を操作し、決定時にシーン遷移またはアプリケーション終了を行うクラス

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceSelector : MonoBehaviour
{
    // 選択時に再生するクリック音
    public AudioClip clickSound;

    // 決定時に再生する確定音
    public AudioClip confirmSound;

    // 効果音の音量（0.0?1.0）
    [Range(0f, 1f)] public float seVolume = 1.0f;

    // 効果音再生用のAudioSource
    private AudioSource audioSource;

    // 上（ゲームスタート）と下（ゲーム終了）の位置座標
    private Vector3 upperPosition;
    private Vector3 lowerPosition;

    // 現在の選択位置が上かどうか（true = 上、false = 下）
    private bool isUpper = true;

    void Start()
    {
        // 初期位置を設定（現在のX,Zを基にYを決定）
        float x = transform.position.x;
        float z = transform.position.z;

        upperPosition = new Vector3(x, -1.6f, z); // ゲームスタート位置
        lowerPosition = new Vector3(x, -3.2f, z); // ゲーム終了位置

        // 初期位置を上に設定
        transform.position = upperPosition;

        // AudioSourceを生成し初期設定
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = seVolume; // ユーザー設定音量
    }

    void Update()
    {
        // ↑キーで上の選択肢に移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayClickSound();
            MoveToUpper();
        }
        // ↓キーで下の選択肢に移動
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayClickSound();
            MoveToLower();
        }

        // Enterキーで選択肢を決定
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayConfirmSound();

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
        transform.position = new Vector3(pos.x, -1.6f, pos.z);
        isUpper = true;
    }

    // 下の選択肢に移動（Y座標を変更）
    void MoveToLower()
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, -3.2f, pos.z);
        isUpper = false;
    }

    // 選択移動時の音を再生
    void PlayClickSound()
    {
        if (clickSound != null) audioSource.PlayOneShot(clickSound);
    }

    // 決定時の音を再生
    void PlayConfirmSound()
    {
        if (confirmSound != null) audioSource.PlayOneShot(confirmSound);
    }

    // アプリケーション終了（エディタ上では再生停止）
    void PlayClickAndQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}