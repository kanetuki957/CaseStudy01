using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteAlways] // ← これでEdit Mode中も動作
public class StageController : MonoBehaviour
{
    public Button targetButton;
    //public string requiredStageName;
    public bool requireClearToUnlock = true;

    public Image lockIconImage;
    public Sprite lockedIcon;   // key01
    public Sprite unlockedIcon; // key02

    private Animator buttonAnimator;

    void OnEnable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Debug.Log($" Edit Mode中に鍵アイコンを仮表示します（{gameObject.name}）");

            if (lockIconImage != null && lockedIcon != null)
            {
                lockIconImage.sprite = lockedIcon;
                lockIconImage.enabled = true;

                // シーンビューとUIを更新
                EditorApplication.QueuePlayerLoopUpdate();
                SceneView.RepaintAll();
            }
            else
            {
                Debug.LogWarning(" Edit Mode中：lockIconImage または lockedIcon が未設定です。");
            }
        }
#endif
    }

    public string[] requiredStageNames; // ← 配列に変更

    void Start()
    {
        if (!Application.isPlaying) return;

        if (targetButton == null)
        {
            Debug.LogError(" ターゲットボタンが設定されていません。");
            return;
        }

        if (StageManager.Instance == null)
        {
            Debug.LogError(" StageManager.Instance が null です。");
            return;
        }

        buttonAnimator = targetButton.GetComponent<Animator>();

        if (requireClearToUnlock)
        {
            bool isUnlocked = true;

            foreach (string stageName in requiredStageNames)
            {
                if (!StageManager.Instance.IsStageCleared(stageName))
                {
                    isUnlocked = false;
                    break;
                }
            }

            Debug.Log($" ステージ「{string.Join(", ", requiredStageNames)}」のクリア状態: {isUnlocked}");

            targetButton.interactable = isUnlocked;

            if (buttonAnimator != null)
            {
                buttonAnimator.SetBool("IsInteractable", isUnlocked);
            }

            if (lockIconImage != null)
            {
                lockIconImage.sprite = isUnlocked ? unlockedIcon : lockedIcon;
                lockIconImage.enabled = true;
            }
        }
        else
        {
            targetButton.interactable = true;

            if (buttonAnimator != null)
            {
                buttonAnimator.SetBool("IsInteractable", true);
            }

            if (lockIconImage != null)
            {
                lockIconImage.enabled = false;
            }
        }
    }

}
