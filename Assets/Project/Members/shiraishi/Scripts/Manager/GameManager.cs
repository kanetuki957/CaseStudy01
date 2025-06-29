using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

// ゲームの状態を管理する列挙体
public enum GameState
{
    Ready,      // 待機中
    Playing,    // ゲーム中
    Paused,     // 一時停止(未実装)
    Editing,    // 編集モード（初期状態）
}

public class GameManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static GameManager Instance { get; private set; }

    // 現在のゲーム状態
    public GameState currentState { get; private set; } = GameState.Editing;
    public event System.Action<GameState, GameState> OnGameStateChanged;

    // ボタン関連
    [SerializeField] private Button resetButton;
    [SerializeField] private Button editButton;
    [SerializeField] private CanvasGroup resetButtonCanvasGroup;
    [SerializeField] private CanvasGroup editButtonCanvasGroup;

    [Header("ボタンクリック時のSE")]
    public AudioClip clickSE;
    public float clickSEVolume = 1f;

    [Header("スタートボタンを押してからリセットボタンが表示されるまでの長さ（秒）")]
    public float turnResetButtonDuration = 1.0f;

    private void Awake()
    {
        // シングルトンの初期化（重複インスタンス防止）
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 最初は非操作状態・半透明
        resetButton.interactable = false;
        resetButtonCanvasGroup.alpha = 0.3f;
        resetButtonCanvasGroup.blocksRaycasts = false;

        // Scene内にEventSystemが存在しなければ
        if (FindObjectOfType<EventSystem>() == null)
        {
            // 新しいGameObjectを生成してEventSystemとStandaloneInputModuleを追加
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToScene("SelectScene");
        }
    }


    // 状態変更用メソッド（外部から状態を更新するために使用）
    public void SetGameState(GameState newState)
    {
        if (currentState != newState)
        {
            var previousState = currentState;
            currentState = newState;
            OnGameStateChanged?.Invoke(previousState, currentState);
        }
    }



    /*** ボタンが押されたときの処理 ***/

    // Startボタンが押されたときの処理
    public void OnStartButtonPressed()
    {
        // SE再生
        if (clickSE != null)
            AudioSource.PlayClipAtPoint(clickSE, Camera.main.transform.position, clickSEVolume);

        SetGameState(GameState.Playing);
        StartCoroutine(ShowResetButtonDelayed());

        editButton.interactable = false;
        editButtonCanvasGroup.alpha = 0.3f;
        editButtonCanvasGroup.blocksRaycasts = false;
    }

    // Startボタンが押されたあと一定時間待ってからResetボタンを表示する処理
    private IEnumerator ShowResetButtonDelayed()
    {
        yield return new WaitForSeconds(turnResetButtonDuration);

        resetButton.interactable = true;
        resetButtonCanvasGroup.alpha = 1.0f;
        resetButtonCanvasGroup.blocksRaycasts = true;
    }

    // Pauseボタンが押されたときの処理(未実装)
    public void OnPauseButtonPressed()
    {
        // SE再生
        if (clickSE != null)
            AudioSource.PlayClipAtPoint(clickSE, Camera.main.transform.position, clickSEVolume);
        SetGameState(GameState.Paused);
    }

    // Resetボタンが押されたときの処理
    public void OnResetButtonPressed()
    {
        // SE再生
        if (clickSE != null)
            AudioSource.PlayClipAtPoint(clickSE, Camera.main.transform.position, clickSEVolume);
        GoToScene(SceneManager.GetActiveScene().name);
    }

    // Editボタンが押されたときの処理
    public void OnEditButtonPressed()
    {
        if (clickSE != null)
            AudioSource.PlayClipAtPoint(clickSE, Camera.main.transform.position, clickSEVolume);
        GoToScene("SelectScene");
    }


    /*** シーン遷移関連 ***/

    [Header("シーン順リスト（ScriptableObject）")]
    public GameSceneOrderList sceneOrderList; // インスペクターでアセットをドラッグして指定

    // リスト内の次のシーン名を取得する
    public string GetNextSceneName()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        int idx = sceneOrderList.sceneNames.IndexOf(currentScene);
        if (idx >= 0 && idx < sceneOrderList.sceneNames.Count - 1)
        {
            return sceneOrderList.sceneNames[idx + 1];
        }
        return null;
    }

    // リスト内の前のシーン名を取得する
    public string GetPreviousSceneName()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        int idx = sceneOrderList.sceneNames.IndexOf(currentScene);
        if (idx > 0)
        {
            return sceneOrderList.sceneNames[idx - 1];
        }
        return null;
    }

    // リストで現在シーンの次にあるシーンをロード
    public bool GoToNextScene()
    {
        string next = GetNextSceneName();
        if (!string.IsNullOrEmpty(next))
        {
            Debug.Log("リストの次のシーンに移動します");
            SceneTransitionHelper.StartTransition(next, this);
            return true;
        }
        return false;
    }

    // シーン名を直接指定してロード
    public void GoToScene(string sceneName)
    {
        Debug.Log("入力されたシーンに移動します");
        SceneTransitionHelper.StartTransition(sceneName, this);
    }
}
