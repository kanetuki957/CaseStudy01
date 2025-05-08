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
        // 子から EffectManager を自動で探す（未設定時のみ）
        if (effectManager == null)
        {
            effectManager = GetComponentInChildren<EffectManager>();
            if (effectManager == null)
            {
                Debug.LogWarning($"[DisappearBlock] 子オブジェクトに EffectManager が見つかりません: {gameObject.name}");
            }
        }
        UpdateState();
    }

    public void Activate()
    {
        isVisible = !isVisible;

        // スプライト切り替え
        spriteRenderer.sprite = isVisible ? visibleSprite : hiddenSprite;

        // 当たり判定の有効/無効
        col.enabled = isVisible;

        // 演出再生
        effectManager?.PlayAllEffects();
    }

    private void UpdateState()
    {
        spriteRenderer.sprite = isVisible ? visibleSprite : hiddenSprite;
        spriteRenderer.color = new Color(1, 1, 1, isVisible ? 1f : 0f);
        col.enabled = isVisible;
    }
}
