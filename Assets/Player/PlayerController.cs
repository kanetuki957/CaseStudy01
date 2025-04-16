using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true; //
    }
        void Update()
    {
        HandleMovement();
        HandleJump();
        HandleSpriteDirection();
    }

    // **�ړ�����**
    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }

    // **�W�����v����**
    void HandleJump()
    {
        //���n����
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.5f);


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
        
        if (!isGrounded && rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * 1.0f * Time.deltaTime;
        }
        if(!isGrounded && rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * 0.1f * Time.deltaTime;
        }
    }

    // **�v���C���[�̌���**
    void HandleSpriteDirection()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (moveX > 0)
        {
            spriteRenderer.flipX = false; // �E����
        }
        else if (moveX < 0)
        {
            spriteRenderer.flipX = true; // ������
        }
    }
}


