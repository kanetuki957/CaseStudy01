using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject[] canvases; // Canvas1, Canvas2, Canvas3 など
    public PageTurnCanvasTransitions canvasTransition; // 任意。未設定でも動作します。

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

        // アニメーション中なら何もしない
        if (canvasTransition != null && canvasTransition.IsAnimating())
            return;

        if (canvasTransition != null)
        {
            canvasTransition.StartCanvasTransition(canvases[currentIndex], targetCanvas);
        }
        else
        {
            canvases[currentIndex].SetActive(false);
            targetCanvas.SetActive(true);
        }

        currentIndex = System.Array.IndexOf(canvases, targetCanvas);
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
