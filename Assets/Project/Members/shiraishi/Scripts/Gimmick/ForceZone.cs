using UnityEngine;
using UnityEngine.UIElements;

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
    private bool isStay = false;

    [SerializeField] private EffectManager effectManager;

    Rigidbody2D rb_player;

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
            rb_player = other.GetComponent<Rigidbody2D>();
            if (rb_player != null)
            {
                rb_player.WakeUp();
                Debug.Log("Player is in the force zone");
                isStay = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isStay = false;
        rb_player = null;
    }

    private void FixedUpdate()
    {
        if (rb_player != null)
        {
            if (isStay)
            {
                if (restrictHorizontalMovement)
                {
                    rb_player.velocity = new Vector2(0, rb_player.velocity.y);
                }
                //rb_player.AddForce(forceDirection.normalized * forceStrength * Time.deltaTime);
                rb_player.velocity += forceDirection.normalized * forceStrength;
            }
            Debug.Log(rb_player.velocity);
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

            isActive = false;
        }
    }

    // デバッグ用矢印描画
    private void OnDrawGizmos()
    {
        // Gizmoの色
        Gizmos.color = Color.cyan;
        // オブジェクトの位置
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        // 方向ベクトル
        Vector3 dir = new Vector3(forceDirection.x, forceDirection.y, 0).normalized;
        // 矢印を描画
        Gizmos.DrawLine(pos, pos + dir);
        // 先端に三角形の矢印を追加
        DrawArrowHead(pos + dir, dir, 0.3f);
    }

    void DrawArrowHead(Vector3 pos, Vector3 dir, float size)
    {
        Vector3 right = Quaternion.Euler(0, 0, 30) * -dir;
        Vector3 left = Quaternion.Euler(0, 0, -30) * -dir;
        Gizmos.DrawLine(pos, pos + right * size);
        Gizmos.DrawLine(pos, pos + left * size);
    }
}
