using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 移動処理
        float moveX = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        // 接地判定（Raycastを使用）
         bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.0f);


        // ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

       
        if (moveX > 0)
        {
            animator.SetBool("Direction", false);
            spriteRenderer.flipX = false; // 右向き
        }
        else if (moveX < 0)
        {
            animator.SetBool("Direction", true);
            spriteRenderer.flipX = true; // 左向き
        }
       

    }

}