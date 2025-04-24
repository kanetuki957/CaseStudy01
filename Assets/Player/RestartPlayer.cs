using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartPlayer : MonoBehaviour
{
    public Button startButton;
    private Vector3 startPosition;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
 
        startPosition = transform.position;  // ‰ŠúˆÊ’u‚ğ‹L˜^
        startButton.onClick.AddListener(ResetPlayerPosition);
    }
    public void ResetPlayerPosition()
    {
        transform.position = startPosition;  // ‰ŠúˆÊ’u‚Ö–ß‚·
        spriteRenderer.flipX = false;
    }
}
