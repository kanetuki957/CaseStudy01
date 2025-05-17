using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えに使用
using UnityEngine.UI;

/// <summary>
/// ボタンがクリックされたときに効果音を鳴らし、音が再生し終わったら指定したシーンに遷移するスクリプト。
/// このスクリプトはUIボタンにアタッチして使用する。
/// </summary>
public class SceneSwitcher : MonoBehaviour
{
    // 遷移先のシーン名（インスペクターで指定）
    public string sceneName = "1stStagePart1Scene";

    // クリック時に再生する効果音
    public AudioClip clickSound;

    // ボタンとAudioSourceの参照
    private Button button;
    private AudioSource audioSource;

    void Start()
    {
        // このオブジェクトにアタッチされている Button コンポーネントを取得
        button = GetComponent<Button>();

        // AudioSource を動的に追加
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 自動再生はしない

        // ボタンがクリックされた時の処理を登録
        if (button != null)
        {
            button.onClick.AddListener(PlaySoundAndSwitchScene);
        }
    }

    /// <summary>
    /// 効果音を再生し、再生が終了したらシーンを切り替える
    /// </summary>
    void PlaySoundAndSwitchScene()
    {
        if (clickSound != null)
        {
            // 効果音を再生し、再生終了後にシーン遷移
            audioSource.PlayOneShot(clickSound);
            Invoke("SwitchScene", clickSound.length); // SEの長さ分だけ待つ
        }
        else
        {
            // SEが設定されてないときは即遷移
            SwitchScene();
        }
    }

    /// <summary>
    /// 指定されたシーンへ遷移する
    /// </summary>
    void SwitchScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}