using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PageTurnTransitions : MonoBehaviour
{
    public RawImage TransitionImage;
    public Material PageTurnMaterial;// 完全に非公開にして、SceneTransitionHelperから使う
    private string nextSceneName => SceneTransitionHelper.NextSceneName;

    [SerializeField] private RawImage nextSceneImage;

    public float duration = 1.0f; // めくり時間
    private float progress = 0f;
    private bool animating = true;

    private void Awake()
    {
        // _MainTexに遷移元画像
        PageTurnMaterial.SetTexture("_MainTex", SceneTransitionHelper.Screenshot);
        TransitionImage.material = PageTurnMaterial;
    }

    void Start()
    {
        // 非同期Additiveで次シーンをロード
        StartCoroutine(LoadNextSceneAdditive());
    }

    void Update()
    {
        if (!animating) return;

        progress += Time.deltaTime / duration;

        // -1 ～ 1 に線形補間（0で中央折り）
        float flipValue = Mathf.Lerp(1f, -1f, progress);

        PageTurnMaterial.SetFloat("_Flip", flipValue);

        if (progress >= 1f)
        {
            animating = false;
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        }
    }

    IEnumerator LoadNextSceneAdditive()
    {
        // Additiveで遷移先をロード
        var async = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        while (!async.isDone)
            yield return null;

        // 次シーンのMainCameraを探す
        Scene nextScene = SceneManager.GetSceneByName(nextSceneName);
        Camera[] allCameras = GameObject.FindObjectsOfType<Camera>();
        Camera nextCam = null;
        foreach (Camera c in allCameras)
        {
            if (c.gameObject.scene == nextScene)
            {
                nextCam = c;
                break;
            }
        }

        if (nextCam == null)
        {
            Debug.LogError("次シーンのカメラが見つかりません");
            yield break;
        }

        // 次シーンのカメラを一時的に非アクティブ化
        nextCam.gameObject.SetActive(true);

        // RenderTextureをセット
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        nextCam.targetTexture = rt;
        nextCam.Render();
        nextCam.targetTexture = null;

        // RenderTexture→Texture2Dへ変換
        RenderTexture.active = rt;
        Texture2D nextSceneTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        nextSceneTex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        nextSceneTex.Apply();
        RenderTexture.active = null;
        Destroy(rt);

        nextSceneImage.texture = nextSceneTex;

        // 遷移先シーンを非表示に
        foreach (GameObject go in nextScene.GetRootGameObjects())
        {
            go.SetActive(false);
        }

        // ページめくりアニメ開始
        animating = true;
        progress = 0;
    }
}
