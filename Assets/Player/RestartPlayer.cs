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
 
        startPosition = transform.position;  // �����ʒu���L�^
        startButton.onClick.AddListener(ResetPlayerPosition);
    }
    public void ResetPlayerPosition()
    {
        transform.position = startPosition;  // �����ʒu�֖߂�
        spriteRenderer.flipX = false;
    }
}
