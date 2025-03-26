using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aitem : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;  // �W�����v�̋���
    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded = false; // �n�ʂɂ��Ă��邩����

    public GameObject itemDisplayPrefab;    // �A�C�e��
    public Transform itemDisplayParent;     // �\���p�I�u�W�F�N�g

    private List<string> collectedItems = new List<string>();// �擾�����A�C�e�����X�g
    private List<GameObject> displayedItems = new List<GameObject>(); // ��ʂɕ\�����̃A�C�e�����X�g

    public List<GameObject> desObject = new List<GameObject>(); // ������I�u�W�F�N�g�̃��X�g
    public float desRange = 3.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        // �X�y�[�X�L�[�ŃW�����v�i�n�ʂɂ���Ƃ��̂݁j
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false; // �W�����v�����̂Œn�ʂ��痣���
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UseItem();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            string itemName = other.gameObject.name;
            collectedItems.Add(itemName);   // �A�C�e�����X�g��ǉ�
            Destroy(other.gameObject);  // �A�C�e���̍폜
            ShowItemDisplay(itemName, 2.5f, 2.1f, 0f);  // �\���p�I�u�W�F�N�g����
        }
    }

    void ShowItemDisplay(string itemName, float x, float y, float z)
    {
        GameObject itemDisplay = Instantiate(itemDisplayPrefab, new Vector3(x, y, z), Quaternion.identity);
        itemDisplay.name = itemName;

        if (itemDisplayParent != null)
        {
            itemDisplay.transform.SetParent(itemDisplayParent);
        }
        displayedItems.Add(itemDisplay);
    }

    void UseItem()
    {
        if (collectedItems.Count > 0)
        {
            string usedItem = collectedItems[0];
            collectedItems.RemoveAt(0);

            Debug.Log(usedItem + "���g�p�����I");

            if (displayedItems.Count > 0)
            {
                Destroy(displayedItems[0]);
                displayedItems.RemoveAt(0);
            }

            foreach (GameObject obj in desObject)
            {
                if (obj != null && Vector3.Distance(transform.position, obj.transform.position) <= desRange)
                {
                    Destroy(obj);
                    Debug.Log(obj.name + "���폜");
                    break;
                }
            }
        }
        else
        {
            Debug.Log("�A�C�e��������܂���!");
        }
    }

    // **�n�ʂɐG�ꂽ�Ƃ��̏���**
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // �n�ʂɂ��Ă���
        }
    }

    // **�n�ʂ��痣�ꂽ�Ƃ��̏���**
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // �n�ʂ��痣�ꂽ
        }
    }
}
