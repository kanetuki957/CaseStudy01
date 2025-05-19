using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float desiredDistance = 2f; // 一定の距離を保つ
    public GameObject[] ignoredPlayers; // 通り抜けたいプレイヤーのリスト
    private Rigidbody rb;
    private Collider myCollider;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        myCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < ignoredPlayers.Length; i++)
        {
            Collider playerCollider = ignoredPlayers[i].GetComponent<Collider>();
            if (playerCollider != null)
            {
                Physics.IgnoreCollision(myCollider, playerCollider);
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 direction = (target.position - rb.position).normalized;
        float distance = Vector3.Distance(target.position, rb.position);
        // スプライトの向きを反転
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // 右向き
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true; // 左向き
        }


        // 一定の距離より近い場合は移動しない
        if (distance > desiredDistance)
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }
    void RestartButton()
    {
       
    }
}
