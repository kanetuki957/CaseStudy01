using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityCharacter : MonoBehaviour
{
    public float moveSpeed = 3f;         // 右移動速度
    public float floatUpSpeed = 5f;      // 上昇速度
    public float upperLimitY = 4.5f;     // 天井位置Y

    public float warpOffsetX = 1.0f; // ワープ位置をX方向にずらす値

    private Rigidbody2D rb;

    // キャラクターの状態（地上 → 浮上 → 張り付き → 天井移動 → ワープ停止）
    private enum State
    {
        Walking,
        FloatingUp,
        StuckToCeiling,
        CeilingMoving,
        Warping
    }

    private State currentState = State.Walking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // 初期は重力なし（完全手動制御）
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Walking:
                // 地面を右に歩く
                rb.velocity = new Vector2(moveSpeed, 0f);
                break;

            case State.FloatingUp:
                // 真上にゆっくり移動（吸い寄せ中）
                rb.velocity = new Vector2(0f, floatUpSpeed);
                if (transform.position.y >= upperLimitY)
                {
                    rb.velocity = Vector2.zero;
                    rb.gravityScale = 0f; // 重力OFF
                    FlipVertically();
                    currentState = State.StuckToCeiling;
                }
                break;

            case State.StuckToCeiling:
                // 少しの遅延後に天井移動を開始（またはすぐ切り替えでもOK）
                currentState = State.CeilingMoving;
                break;

            case State.CeilingMoving:
                // 天井上で右へ移動
                rb.velocity = new Vector2(moveSpeed, 0f);
                break;

            case State.Warping:
                // 完全に移動停止（ワープ直後）
                rb.velocity = Vector2.zero;
                break;
        }
    }

    // 上昇を開始する関数
    public void TriggerFloatUp()
    {
        if (currentState == State.Walking)
        {
            currentState = State.FloatingUp;
        }
    }

    private void FlipVertically()
    {
        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;
    }

    // 他オブジェクトから呼ばれて瞬間移動する関数
    public void WarpTo(GameObject target)
    {
        if (target == null) return;

        // ワープ先の座標を取得して、X方向にオフセットを加える
        Vector3 targetPos = target.transform.position;
        targetPos.x += warpOffsetX;

        // キャラをずらした位置にワープ
        transform.position = targetPos;

        // 状態切り替え（ワープ後停止）
        currentState = State.Warping;
        rb.velocity = Vector2.zero;
    }
}