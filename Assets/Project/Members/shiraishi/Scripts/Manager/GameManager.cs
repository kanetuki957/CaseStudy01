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
    Ready,      // 開始前（初期状態）
    Playing,    // ゲーム中
    Paused,     // 一時停止中
    Cleared,    // クリア済み
    GameOver    // ゲームオーバー
}

public class GameManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static GameManager Instance { get; private set; }

    // 現在のゲーム状態
    public GameState CurrentState { get; private set; } = GameState.Ready;

    [SerializeField] private Button resetButton;
    [SerializeField] private CanvasGroup resetButtonGroup;

    [Header("ボタンクリック時のSE")]
    public AudioClip clickSE;
    public float clickSEVolume = 1f;

    [Header("リセット時に鳴らすSE")]
    public AudioClip resetSE;
    public float seVolume = 1f;

    [Header("リセットボタンが表示されるまでの長さ（秒）")]
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
        //DontDestroyOnLoad(this);

        // 最初は非操作状態・透明に近い
        resetButton.interactable = false;
        resetButtonGroup.alpha = 0.3f;
        resetButtonGroup.blocksRaycasts = false;

        // Scene内にEventSystemが存在しなければ
        if (FindObjectOfType<EventSystem>() == null)
        {
            // 新しいGameObjectを生成してEventSystemとStandaloneInputModuleを追加
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

    }

    // 状態変更用メソッド（外部から状態を更新するために使用）
    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
    }

    // Startボタンが押されたときに呼ばれる（ゲーム開始）
    public void OnStartButtonPressed()
    {
        // SE再生
        if (clickSE != null)
            AudioSource.PlayClipAtPoint(clickSE, Camera.main.transform.position, clickSEVolume);

        SetGameState(GameState.Playing);
        StartCoroutine(ShowPlayButtonDelayed());
    }

    private IEnumerator ShowPlayButtonDelayed()
    {
        yield return new WaitForSeconds(turnResetButtonDuration);

        // 最初は非操作状態・透明に近い
        resetButton.interactable = true;
        resetButtonGroup.alpha = 1.0f;
        resetButtonGroup.blocksRaycasts = true;
    }

    // Pauseボタンが押されたときに呼ばれる（一時停止）(未実装)
    public void OnPauseButtonPressed()
    {
        // SE再生
        if (clickSE != null)
            AudioSource.PlayClipAtPoint(clickSE, Camera.main.transform.position, clickSEVolume);
        SetGameState(GameState.Paused);
    }

    // Resetボタンが押されたときに呼ばれる（リスタート処理）
    public void OnResetButtonPressed()
    {
        // SE再生
        if (clickSE != null)
            AudioSource.PlayClipAtPoint(clickSE, Camera.main.transform.position, clickSEVolume);
        StartCoroutine(DoFadeAndReload());
    }

    // フェード演出 + SE + シーンリロードを順に行う
    private IEnumerator DoFadeAndReload()
    {
        // 効果音を再生（もし設定されていれば）
        if (resetSE != null)
        {
            AudioSource.PlayClipAtPoint(resetSE, Camera.main.transform.position, seVolume);
            yield return new WaitForSeconds(0.2f); // 音の余韻を待つ
        }

        // 現在のシーンを再読み込み
        GameManager.Instance.GoToScene(SceneManager.GetActiveScene().name);
    }

    /** シーン遷移関連 **/


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
    public void GoToNextScene()
    {
        string next = GetNextSceneName();
        if (!string.IsNullOrEmpty(next))
        {
            SceneTransitionHelper.StartTransition(next, this);
        }
    }

    // シーン名を直接指定してロード
    public void GoToScene(string sceneName)
    {
        SceneTransitionHelper.StartTransition(sceneName, this);
    }
}
