using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public Button startButton;
    public Button restartButton; 
    public Collider targetBlockCollider;


    private bool Button = false;
    private bool playerDirection = true;
    private bool isGrounded;
    private bool isWall;
    private float playerWidth;
    private Vector3 move;
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private float moveX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startButton.onClick.AddListener(MoveButton);
        restartButton.onClick.AddListener(RestartButton);

        playerWidth = spriteRenderer.bounds.size.x;  // �X�v���C�g�̕����擾
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

    void RestartButton()
    {
        enabled = true;  // �X�N���v�g�̓�����~
        Button = false;
        playerDirection = true;
    }
    // **�ړ�����**
    void HandleMovement()
    {
        Vector3 direction = playerDirection? Vector3.right : Vector3.left;
        Vector3 rayOrigin = transform.position + Vector3.up * 1f; // ���̈ʒu���甭��
        Vector3 rayOrigin1 = transform.position + Vector3.down * 0.5f; // �������烌�C���΂�

        // �O���ɕǂ����邩�`�F�b�N
        if (Physics.Raycast(rayOrigin, direction, 0.5f))
        {
          playerDirection = !playerDirection;  // ���]
        }

        RaycastHit hit;
        //if (Physics.Raycast(rayOrigin1, Vector3.down, out hit, 1f))
        //{

        //    float slopeAngle = Vector3.Angle(hit.normal, Vector3.up); // �n�ʂ̌X�Ίp���擾
        //    //Debug.Log("�X�Ίp: " + slopeAngle);
        //    //rb.velocity += Vector3.down * 2f * Time.deltaTime;  // �d�͂����߂�

        //}
        if (Physics.Raycast(rayOrigin1, Vector3.down, out hit, 10f))
        { // �ő�10m�͈̔͂Ŕ���
            float groundDistance = Mathf.Round(hit.distance* 10.0f);
           
            //Debug.Log("���܂ł̋���: " + groundDistance);
            rb.velocity += Vector3.down * 2f * Time.deltaTime;
        }





      
        


        //�����ɂ���Đi�ޕ�����ς���
       if (playerDirection)
       {
               moveX = +1.0f;
       }
       else
       {
            moveX = -1.0f;
       }
      
        move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);


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
      //���n�̕\����ς���
      if (playerDirection)
        {
            spriteRenderer.flipX = false; // ������
        }
        else
        {
            spriteRenderer.flipX = true; // �E����
        }
    }
 
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == targetBlockCollider)
        {
            Debug.Log("Touched specific block!");
            enabled = false;  // �X�N���v�g�̓�����~
        }
    }
}


