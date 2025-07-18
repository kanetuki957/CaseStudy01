using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffectSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum EffectMode { Color, Outline }
    public EffectMode currentMode = EffectMode.Color;

    public Image buttonImage;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;

    public Outline outline;
    public Color outlineHoverColor = Color.cyan;

    private void Start()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();

        if (outline == null)
            outline = GetComponent<Outline>();

        ApplyNormalState();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentMode == EffectMode.Color)
        {
            if (buttonImage != null)
                buttonImage.color = hoverColor;
        }
        else if (currentMode == EffectMode.Outline)
        {
            if (outline != null)
            {
                outline.enabled = true;
                outline.effectColor = outlineHoverColor;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ApplyNormalState();
    }

    private void ApplyNormalState()
    {
        if (buttonImage != null)
            buttonImage.color = normalColor;

        if (outline != null)
            outline.enabled = false;
    }

    // 外部からモードを切り替える用の関数
    public void SetEffectMode(EffectMode mode)
    {
        currentMode = mode;
        ApplyNormalState();
    }
}
