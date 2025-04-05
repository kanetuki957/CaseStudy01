using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour
{
    private Renderer objRenderer;
    private Color originalColor;
    public Color highlightColor = Color.yellow;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        originalColor = objRenderer.material.color;
    }

    void OnMouseEnter()
    {
        objRenderer.material.color = highlightColor;
    }

    void OnMouseExit()
    {
        objRenderer.material.color = originalColor;
    }
}
