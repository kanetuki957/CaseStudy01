using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceSelector : MonoBehaviour
{
    public AudioClip clickSound;
    public SceneTransitionAnimator animator;
    private AudioSource audioSource;

    private Vector3 upperPosition;
    private Vector3 lowerPosition;

    private bool isUpper = true;

    void Start()
    {
        float x = transform.position.x;
        float z = transform.position.z;

        upperPosition = new Vector3(x, -1.1f, z);
        lowerPosition = new Vector3(x, -3.4f, z);

        transform.position = upperPosition;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        audioSource.volume = 1.0f;
    }

    void Update()
    {
        // 上下キー入力で位置変更
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayClickSound();
            MoveToUpper();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayClickSound();
            MoveToLower();
        }

        // Enterキーで処理実行
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayClickSound();

            if (isUpper)
            {
                StartCoroutine(PlayClickAndTransition("SelectScene"));
            }
            else
            {
                PlayClickAndQuit();
            }
        }
    }

    void MoveToUpper()
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, -1.1f, pos.z);
        isUpper = true;
    }

    void MoveToLower()
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, -3.4f, pos.z);
        isUpper = false;
    }

    void PlayClickSound()
    {
        if (clickSound != null) audioSource.PlayOneShot(clickSound);
    }

    IEnumerator PlayClickAndTransition(string sceneName)
    {
        if (animator != null)
        {
            yield return animator.AnimateAndSwitch(() =>
            {
                SceneManager.LoadScene(sceneName);
            });
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    void PlayClickAndQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}