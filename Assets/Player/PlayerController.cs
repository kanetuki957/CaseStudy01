using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public Button startButton;


    private bool Button = false;
    private bool playerDirection = true;
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool isWall;
    private Vector3 move;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startButton.onClick.AddListener(MoveButton);
         rb.freezeRotation = true; //
    }
        void Update()
    {
        if (Button)
        {
            HandleMovement();
        }
      
        HandleSpriteDirection();
    }

    void MoveButton()
    {
        Button = true;
    }

    // **�ړ�����**
    void HandleMovement()
    {
        Vector3 direction = playerDirection? Vector3.right : Vector3.left;

        // �O���ɕǂ����邩�`�F�b�N
        if (Physics.Raycast(transform.position, direction, 0.5f))
        {
          playerDirection = !playerDirection;  // ���]
        }

        if (playerDirection)
        {
            float moveX = +1.0f;
            move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
        else
        {
            float moveX = -1.0f;
            move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
      //RaycastHit hit;
      //      if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
      //      {
      //          float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
      //          float speedMultiplier = Mathf.Lerp(1f, 0.5f, slopeAngle / 45f);
      //          MoveCharacter(speedMultiplier);
      //      }
      //        void MoveCharacter(float speedMultiplier)
      //  {
      //      float moveSpeed = 5f * speedMultiplier;
      //      transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
      //  }


    }
   

    // **�W�����v����**
    //void HandleJump()
    //{
    //    //���n����
    //    isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.5f);


    //    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    //    {
    //        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    //    }
        
    //    if (!isGrounded && rb.velocity.y < 0)
    //    {
    //        rb.velocity += Vector3.up * Physics.gravity.y * 1.0f * Time.deltaTime;
    //    }
    //    if(!isGrounded && rb.velocity.y > 0)
    //    {
    //        rb.velocity += Vector3.up * Physics.gravity.y * 0.1f * Time.deltaTime;
    //    }
    //}

    // **�v���C���[�̌���**
    void HandleSpriteDirection()
    {
      
        if (playerDirection)
        {
            spriteRenderer.flipX = false; // �E����
        }
        else
        {
            spriteRenderer.flipX = true; // ������
        }
    }
}


