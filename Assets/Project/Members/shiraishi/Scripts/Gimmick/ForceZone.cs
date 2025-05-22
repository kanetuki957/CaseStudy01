using UnityEngine;

public class ForceZone : MonoBehaviour
{
    [Header("加える方向（正規化されます）")]
    public Vector2 forceDirection = Vector2.up;

    [Header("加える力の強さ")]
    public float forceStrength = 10f;

    [Header("横移動を制限するか")]
    public bool restrictHorizontalMovement = false;

    [SerializeField] private EffectManager effectManager;

    private void Start()
    {
        // 子から EffectManager を自動で探す（未設定時のみ）
        if (effectManager == null)
        {
            effectManager = GetComponentInChildren<EffectManager>();
            if (effectManager == null)
            {
                Debug.LogWarning($"[DisappearBlock] 子オブジェクトに EffectManager が見つかりません: {gameObject.name}");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(forceDirection.normalized * forceStrength);

                if (restrictHorizontalMovement)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
        }
    }
}
