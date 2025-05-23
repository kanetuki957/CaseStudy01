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
    public string sceneName = "1-table"; // 遷移先のシーン名
    public AudioClip clickSound;                    // クリック時の効果音

    private Button button;
    private AudioSource audioSource;
    public SceneTransitionAnimator animator;       // 遷移時のアニメーター

    void Start()
    {
        button = GetComponent<Button>();
        animator = GetComponent<SceneTransitionAnimator>();

        // AudioSourceの設定
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // ボタンが押されたら演出を実行
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                PlaySoundAndAnimate();
            });
        }
    }

    // 効果音とアニメーションを再生してシーン切り替え
    void PlaySoundAndAnimate()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        if (animator != null)
        {
            // アニメーションしてからシーン遷移
            StartCoroutine(animator.AnimateAndSwitch(() =>
            {
                SceneManager.LoadScene(sceneName);
            }));
        }

        else
        {
            // アニメーターがなければ即シーン遷移
            SceneManager.LoadScene(sceneName);
        }
    }
}