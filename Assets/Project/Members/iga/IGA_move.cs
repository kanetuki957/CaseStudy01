using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGA_move : MonoBehaviour
{

    [SerializeField] float speed = 5.0f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   // Rigidbody‚ğæ“¾
    }

    // Update is called once per frame
    void Update()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            move = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            move = 1f;
        }

        // Rigidbody‚Ì‘¬“x‚ğİ’è
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
    }
}
