using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PageTurnCanvasTransitions : MonoBehaviour
{
    public enum PageTurnMode
    {
        MaterialFlip,
        SpriteSequence
    }

    public enum PageTurnDirection
    {
        Forward, 
        Backward 
    }

    [Header("Transition Settings")]
    public PageTurnMode transitionMode = PageTurnMode.MaterialFlip;
    public GameObject transitionCanvasPrefab;
    public Material pageTurnMaterial;
    public Sprite[] pageSprites; // SpriteSequence用
    public float duration = 1.0f;
    public float frameDuration = 0.1f; // SpriteSequence用

    [Header("Camera Settings")]
    public Camera UICamera;

    private GameObject transitionCanvasInstance;
    private RawImage previousSceneImage;
    private RawImage nextSceneImage;
    private Image spriteImage;

    private GameObject currentCanvas;
    private GameObject nextCanvas;
    private float progress = 0f;
    private bool animating = false;
    private PageTurnDirection currentDirection;

    public bool IsAnimating() => animating;

    public void StartCanvasTransition(GameObject fromCanvas, GameObject toCanvas,
        PageTurnDirection direction)
    {
        currentCanvas = fromCanvas;
        nextCanvas = toCanvas;
        currentDirection = direction;
        StartCoroutine(LoadAndAnimate());
    }

    private IEnumerator LoadAndAnimate()
    {
        transitionCanvasInstance = Instantiate(transitionCanvasPrefab);

        if (transitionMode == PageTurnMode.MaterialFlip)
        {
            previousSceneImage = transitionCanvasInstance.transform.Find("previousSceneImage")?.GetComponent<RawImage>();
            nextSceneImage = transitionCanvasInstance.transform.Find("nextSceneImage")?.GetComponent<RawImage>();

            if (previousSceneImage == null || nextSceneImage == null)
            {
                Debug.LogError("RawImage が見つかりません。");
                yield break;
            }

            yield return new WaitForEndOfFrame();
            Texture2D fromTex = CaptureCanvasWithCamera(UICamera, Screen.width, Screen.height);
            previousSceneImage.texture = fromTex;

            nextCanvas.SetActive(true);
            Canvas.ForceUpdateCanvases();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            Texture2D toTex = CaptureCanvasWithCamera(UICamera, Screen.width, Screen.height);
            nextCanvas.SetActive(false);
            nextSceneImage.texture = toTex;

            previousSceneImage.material = new Material(pageTurnMaterial);
            previousSceneImage.material.SetTexture("_MainTex", fromTex);
            previousSceneImage.material.SetFloat("_Flip", currentDirection == 
                PageTurnDirection.Forward ? 1f : -1f);

            progress = 0f;
            animating = true;
        }
        else if (transitionMode == PageTurnMode.SpriteSequence)
        {
            spriteImage = transitionCanvasInstance.GetComponentInChildren<Image>();
            if (spriteImage == null || pageSprites.Length == 0)
            {
                Debug.LogError("Image または Sprite が設定されていません。");
                yield break;
            }

            spriteImage.enabled = false;
            spriteImage.color = new Color(1f, 1f, 1f, 0f); // 透明にする
            animating = true;

            spriteImage.enabled = true;
            spriteImage.color = new Color(1f, 1f, 1f, 1f); // 表示時に戻す


            if (currentDirection == PageTurnDirection.Forward)
            {
                for (int i = 0; i < pageSprites.Length; i++)
                {
                    spriteImage.sprite = pageSprites[i];
                    yield return new WaitForSeconds(frameDuration);
                }
            }
            else
            {
                for (int i = pageSprites.Length - 1; i >= 0; i--)
                {
                    spriteImage.sprite = pageSprites[i];
                    yield return new WaitForSeconds(frameDuration);
                }
            }


            currentCanvas.SetActive(false);
            nextCanvas.SetActive(true);
            Destroy(transitionCanvasInstance);
            animating = false;
        }

    }

    private void Update()
    {
        if (!animating || transitionMode != PageTurnMode.MaterialFlip || previousSceneImage == null) return;

        float flipValue = currentDirection == PageTurnDirection.Forward
         ? Mathf.Lerp(1f, -1f, progress)
         : Mathf.Lerp(-1f, 1f, progress);

        previousSceneImage.material.SetFloat("_Flip", flipValue);

        if (progress >= 1f)
        {
            animating = false;
            currentCanvas.SetActive(false);
            nextCanvas.SetActive(true);
            Destroy(transitionCanvasInstance);
        }
    }


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

        return tex;
    }

}
