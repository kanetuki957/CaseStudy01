using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    public float slideSpeed = 2f;

    void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.collider.attachedRigidbody;

        if (rb != null)
        {
            // 横方向だけを強制的に変更（垂直方向は維持）
            rb.velocity = new Vector2(-slideSpeed, rb.velocity.y);
        }
    }
}
