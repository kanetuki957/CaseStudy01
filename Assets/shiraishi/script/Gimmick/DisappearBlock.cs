using UnityEngine;

public class DisappearBlock : MonoBehaviour, IActivatable
{
    [Header("�\����Ԃ̐؂�ւ��Ɏg���X�v���C�g")]
    public Sprite visibleSprite;    // �\����
    public Sprite hiddenSprite;     // ��\����

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
            col.enabled = isVisible; // �����蔻����؂�ւ�
        }
    }
}
