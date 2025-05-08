using UnityEngine;

public class DisappearBlock : MonoBehaviour, IActivatable
{
    public Sprite visibleSprite;
    public Sprite hiddenSprite;

    [SerializeField] private EffectManager effectManager;

    private bool isVisible = true;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        // �q���� EffectManager �������ŒT���i���ݒ莞�̂݁j
        if (effectManager == null)
        {
            effectManager = GetComponentInChildren<EffectManager>();
            if (effectManager == null)
            {
                Debug.LogWarning($"[DisappearBlock] �q�I�u�W�F�N�g�� EffectManager ��������܂���: {gameObject.name}");
            }
        }
        UpdateState();
    }

    public void Activate()
    {
        isVisible = !isVisible;

        // �X�v���C�g�؂�ւ�
        spriteRenderer.sprite = isVisible ? visibleSprite : hiddenSprite;

        // �����蔻��̗L��/����
        col.enabled = isVisible;

        // ���o�Đ�
        effectManager?.PlayAllEffects();
    }

    private void UpdateState()
    {
        spriteRenderer.sprite = isVisible ? visibleSprite : hiddenSprite;
        spriteRenderer.color = new Color(1, 1, 1, isVisible ? 1f : 0f);
        col.enabled = isVisible;
    }
}
