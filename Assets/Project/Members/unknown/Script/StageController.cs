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
    public string requiredStageName;
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
            bool isUnlocked = StageManager.Instance.IsStageCleared(requiredStageName);
            Debug.Log($" ステージ「{requiredStageName}」のクリア状態: {isUnlocked}");

            targetButton.interactable = isUnlocked;
            Debug.Log($" ボタンの interactable を {isUnlocked} に設定しました。");

            if (buttonAnimator != null)
            {
                buttonAnimator.SetBool("IsInteractable", isUnlocked);
                Debug.Log(" Animator の IsInteractable を設定しました。");
            }

            if (lockIconImage != null)
            {
                lockIconImage.sprite = isUnlocked ? unlockedIcon : lockedIcon;
                lockIconImage.enabled = true;
                Debug.Log($" 鍵アイコンを {(isUnlocked ? "開いた鍵" : "閉じた鍵")} に設定しました。");
            }
            else
            {
                Debug.LogWarning(" lockIconImage が設定されていません！");
            }
        }
        else
        {
            targetButton.interactable = true;
            Debug.Log(" requireClearToUnlock が false のため、ボタンを常に有効にしました。");

            if (buttonAnimator != null)
            {
                buttonAnimator.SetBool("IsInteractable", true);
            }

            if (lockIconImage != null)
            {
                lockIconImage.enabled = false;
                Debug.Log(" 鍵アイコンを非表示にしました。");
            }
        }
    }
}
