using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image arrowImage; // UIButtonの右側に配置されたImage
    public Sprite[] arrowSprites; // やじるし_0000～0005
    public float frameDuration = 0.1f;

    private Coroutine animationCoroutine;

    void Start()
    {
        arrowImage.gameObject.SetActive(false); // 初期状態は非表示
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        arrowImage.gameObject.SetActive(true);
        animationCoroutine = StartCoroutine(LoopArrowAnimation());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        arrowImage.gameObject.SetActive(false);
        animationCoroutine = null;
    }

    private IEnumerator LoopArrowAnimation()
    {
        int index = 0;
        while (true)
        {
            arrowImage.sprite = arrowSprites[index];
            index = (index + 1) % arrowSprites.Length;
            yield return new WaitForSeconds(frameDuration);
        }
    }
}
