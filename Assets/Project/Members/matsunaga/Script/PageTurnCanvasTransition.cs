using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PageTurnCanvasTransitions : MonoBehaviour
{
    public string transitionCanvasPrefabPath = "Prefabs/TransitionSceneCanvas";
    public Material pageTurnMaterial;
    public float duration = 1.0f;

    private GameObject transitionCanvasInstance;
    private RawImage previousSceneImage;
    private RawImage nextSceneImage;

    private GameObject currentCanvas;
    private GameObject nextCanvas;
    private float progress = 0f;
    private bool animating = false;

    public void StartCanvasTransition(GameObject fromCanvas, GameObject toCanvas)
    {
        currentCanvas = fromCanvas;
        nextCanvas = toCanvas;

        StartCoroutine(LoadAndAnimate());
    }

    private IEnumerator LoadAndAnimate()
    {
        // 1. Prefabを読み込んで生成
        GameObject prefab = Resources.Load<GameObject>(transitionCanvasPrefabPath);
        if (prefab == null)
        {
            Debug.LogError("TransitionSceneCanvas プレハブが Resources に見つかりません。");
            yield break;
        }

        transitionCanvasInstance = Instantiate(prefab);
        previousSceneImage = transitionCanvasInstance.transform.Find("previousSceneImage")?.GetComponent<RawImage>();
        nextSceneImage = transitionCanvasInstance.transform.Find("nextSceneImage")?.GetComponent<RawImage>();

        if (previousSceneImage == null || nextSceneImage == null)
        {
            Debug.LogError("RawImage が TransitionSceneCanvas 内に見つかりません。");
            yield break;
        }

        // 2. 現在のCanvasをキャプチャ
        yield return new WaitForEndOfFrame();
        Texture2D fromTex = ScreenCapture.CaptureScreenshotAsTexture();
        previousSceneImage.texture = fromTex;

        // 3. 次のCanvasを一時的に表示してキャプチャ
        nextCanvas.SetActive(true);
        yield return new WaitForEndOfFrame();
        Texture2D toTex = ScreenCapture.CaptureScreenshotAsTexture();
        nextCanvas.SetActive(false);
        nextSceneImage.texture = toTex;

        // 4. マテリアル設定とアニメーション開始
        previousSceneImage.material = new Material(pageTurnMaterial); // インスタンス化
        previousSceneImage.material.SetTexture("_MainTex", fromTex);
        previousSceneImage.material.SetFloat("_Flip", 1f);

        progress = 0f;
        animating = true;
    }

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
}
