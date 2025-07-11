using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject[] canvases; // Canvas1, Canvas2, Canvas3 など
    public PageTurnCanvasTransitions canvasTransition;

    private int currentIndex = 0;

    void Start()
    {
        // 最初の Canvas を表示
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i].SetActive(i == currentIndex);
        }
    }

    public void ShowCanvas(GameObject targetCanvas)
    {
        if (targetCanvas == canvases[currentIndex])
            return;

        if (canvasTransition != null && canvasTransition.IsAnimating())
            return;

        int targetIndex = System.Array.IndexOf(canvases, targetCanvas);
        if (targetIndex == -1)
        {
            Debug.LogError("指定された Canvas が配列に存在しません。");
            return;
        }

        // アニメーション方向を判定
        var direction = targetIndex > currentIndex
            ? PageTurnCanvasTransitions.PageTurnDirection.Forward
            : PageTurnCanvasTransitions.PageTurnDirection.Backward;

        if (canvasTransition != null)
        {
            canvasTransition.StartCanvasTransition(canvases[currentIndex], targetCanvas, direction);
        }
        else
        {
            canvases[currentIndex].SetActive(false);
            targetCanvas.SetActive(true);
        }

        currentIndex = targetIndex;
    }

    public void ShowNextCanvas()
    {
        int nextIndex = (currentIndex + 1) % canvases.Length;
        ShowCanvas(canvases[nextIndex]);
    }

    public void ShowPreviousCanvas()
    {
        int prevIndex = (currentIndex - 1 + canvases.Length) % canvases.Length;
        ShowCanvas(canvases[prevIndex]);
    }
}
