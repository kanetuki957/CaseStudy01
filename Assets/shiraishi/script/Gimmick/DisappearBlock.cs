using UnityEngine;

public class DisappearBlock : MonoBehaviour, IActivatable
{
    [Header("表示状態の切り替えに使うスプライト")]
    public Sprite visibleSprite;    // 表示中
    public Sprite hiddenSprite;     // 非表示中

    private bool isVisible = true;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        UpdateState();
    }

    public void Activate()
    {
        isVisible = !isVisible;
        UpdateState();
    }

    private void UpdateState()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isVisible ? visibleSprite : hiddenSprite;
        }

        if (col != null)
        {
            col.enabled = isVisible; // 当たり判定も切り替え
        }
    }
}
