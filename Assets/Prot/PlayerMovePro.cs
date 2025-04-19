using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovePro : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveDir = Vector2.right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = moveDir * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("open")) // ï«Ç∆Ç‘Ç¬Ç©Ç¡ÇΩÇÁ
        {
            moveDir = -moveDir; // êiçsï˚å¸ÇîΩì]
        }

    }
}
