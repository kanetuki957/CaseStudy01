using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DebugMover : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        if (move > 0.0) { move = 1.0f; } // ‰E•ûŒü
        else if (move < 0.0) { move = -1.0f; } // ¶•ûŒü
        else { move = 0.0f; } // ’âŽ~
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
    }
}
