using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aitem : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;  // ジャンプの強さ
    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded = false; // 地面についているか判定

    public GameObject itemDisplayPrefab;    // アイテム
    public Transform itemDisplayParent;     // 表示用オブジェクト

    private List<string> collectedItems = new List<string>();// 取得したアイテムリスト
    private List<GameObject> displayedItems = new List<GameObject>(); // 画面に表示中のアイテムリスト

    public List<GameObject> desObject = new List<GameObject>(); // 消えるオブジェクトのリスト
    public float desRange = 3.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        // スペースキーでジャンプ（地面にいるときのみ）
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false; // ジャンプしたので地面から離れる
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
            collectedItems.Add(itemName);   // アイテムリストを追加
            Destroy(other.gameObject);  // アイテムの削除
            ShowItemDisplay(itemName, 2.5f, 2.1f, 0f);  // 表示用オブジェクト生成
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

            Debug.Log(usedItem + "を使用した！");

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
                    Debug.Log(obj.name + "を削除");
                    break;
                }
            }
        }
        else
        {
            Debug.Log("アイテムがありません!");
        }
    }

    // **地面に触れたときの処理**
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // 地面についている
        }
    }

    // **地面から離れたときの処理**
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // 地面から離れた
        }
    }
}
