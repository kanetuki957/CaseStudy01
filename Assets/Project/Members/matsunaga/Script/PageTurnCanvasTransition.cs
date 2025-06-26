using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PageTurnCanvasTransitions : MonoBehaviour
{
    [Header("Transition Settings")]
    public GameObject transitionCanvasPrefab; // ページめくり用UIプレハブ（RawImage2枚）
    public Material pageTurnMaterial;         // ページめくり用マテリアル（_Flip使用）
    public float duration = 1.0f;             // アニメーション時間（秒）

    [Header("Camera Settings")]
    public Camera UICamera;                   // UI専用カメラ（Canvasの描画用）

    // 内部状態
    private GameObject transitionCanvasInstance;
    private RawImage previousSceneImage;
    private RawImage nextSceneImage;

    private GameObject currentCanvas;
    private GameObject nextCanvas;
    private float progress = 0f;
    private bool animating = false;

    /// <summary>
    /// アニメーション中かどうかを返す
    /// </summary>
    public bool IsAnimating()
    {
        return animating;
    }

    /// <summary>
    /// Canvas切り替えアニメーションを開始
    /// </summary>
    public void StartCanvasTransition(GameObject fromCanvas, GameObject toCanvas)
    {
        currentCanvas = fromCanvas;
        nextCanvas = toCanvas;
        StartCoroutine(LoadAndAnimate());
    }

    /// <summary>
    /// Canvasをキャプチャしてページめくりアニメーションを実行
    /// </summary>
    private IEnumerator LoadAndAnimate()
    {
        // プレハブのインスタンス化
        if (transitionCanvasPrefab == null)
        {
            Debug.LogError("TransitionSceneCanvas プレハブが割り当てられていません。");
            yield break;
        }

        transitionCanvasInstance = Instantiate(transitionCanvasPrefab);
        previousSceneImage = transitionCanvasInstance.transform.Find("previousSceneImage")?.GetComponent<RawImage>();
        nextSceneImage = transitionCanvasInstance.transform.Find("nextSceneImage")?.GetComponent<RawImage>();

        if (previousSceneImage == null || nextSceneImage == null)
        {
            Debug.LogError("RawImage が TransitionSceneCanvas 内に見つかりません。");
            yield break;
        }

        // 現在のCanvasをキャプチャ
        yield return new WaitForEndOfFrame();
        Texture2D fromTex = CaptureCanvasWithCamera(UICamera, Screen.width, Screen.height);
        previousSceneImage.texture = fromTex;

        // 次のCanvasを一時的に表示してキャプチャ
        nextCanvas.SetActive(true);
        Canvas.ForceUpdateCanvases();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Texture2D toTex = CaptureCanvasWithCamera(UICamera, Screen.width, Screen.height);
        nextCanvas.SetActive(false);
        nextSceneImage.texture = toTex;

        // マテリアル設定とアニメーション開始
        previousSceneImage.material = new Material(pageTurnMaterial);
        previousSceneImage.material.SetTexture("_MainTex", fromTex);
        previousSceneImage.material.SetFloat("_Flip", 1f);

        progress = 0f;
        animating = true;
    }

    /// <summary>
    /// アニメーション進行処理
    /// </summary>
    private void Update()
    {
        if (!animating || previousSceneImage == null) return;

        progress += Time.deltaTime / duration;
        float flipValue = Mathf.Lerp(1f, -1f, progress);
        previousSceneImage.material.SetFloat("_Flip", flipValue);

        if (progress >= 1f)
        {
            animating = false;
            currentCanvas.SetActive(false);
            nextCanvas.SetActive(true);
            Destroy(transitionCanvasInstance);
        }
    }

    /// <summary>
    /// 指定カメラでCanvasをキャプチャしてTexture2Dを返す
    /// </summary>
    private Texture2D CaptureCanvasWithCamera(Camera uiCamera, int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        uiCamera.targetTexture = rt;

        bool wasActive = uiCamera.gameObject.activeSelf;
        uiCamera.gameObject.SetActive(true);
        uiCamera.Render();

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        uiCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        uiCamera.gameObject.SetActive(wasActive);

        Debug.Log("Captured texture size: " + tex.width + "x" + tex.height);
        return tex;
    }
}
