using UnityEngine;

public class ForceZone : MonoBehaviour, IActivatable
{
    [Header("加える方向（正規化されます）")]
    public Vector2 forceDirection = Vector2.up;

    [Header("加える力の強さ")]
    public float forceStrength = 10f;

    private float StartForceStrength = 0f;

    [Header("横移動を制限するか")]
    public bool restrictHorizontalMovement = false;

    [Header("レバーを起動したときの挙動\ntrue : 反転, false : 停止")]
    public bool isTurn = false;

    private bool isActive = false;  //  起動中かどうか

    [SerializeField] private EffectManager effectManager;

    Rigidbody2D rb;

    private void Start()
    {
        // 子から EffectManager を自動で探す（未設定時のみ）
        if (effectManager == null)
        {
            effectManager = GetComponentInChildren<EffectManager>();
            if (effectManager == null)
            {
                //Debug.LogWarning($"[DisappearBlock] 子オブジェクトに EffectManager が見つかりません: {gameObject.name}");
            }
        }

        StartForceStrength = forceStrength;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(forceDirection.normalized * forceStrength);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        rb = null;
    }

    private void FixedUpdate()
    {
        if (restrictHorizontalMovement && rb != null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    public void Activate()
    {
        if (!isActive)
        {
            if (isTurn)
            {
                forceDirection = -forceDirection;
            }
            else
            {
                forceStrength = 0;
                GetComponent<BoxCollider2D>().enabled = false;
            }

            isActive = true;
        }
        else
        {
            if (isTurn)
            {
                forceDirection = -forceDirection;
            }
            else
            {
                forceStrength = StartForceStrength;
                GetComponent<BoxCollider2D>().enabled = true;
            }

            isActive= false;
        }
    }
}
