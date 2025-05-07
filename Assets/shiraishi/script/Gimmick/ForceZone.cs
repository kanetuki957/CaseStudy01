using UnityEngine;

public class ForceZone : MonoBehaviour
{
    [Header("����������i���K������܂��j")]
    public Vector2 forceDirection = Vector2.up;

    [Header("������͂̋���")]
    public float forceStrength = 10f;

    [Header("���ړ��𐧌����邩")]
    public bool restrictHorizontalMovement = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(forceDirection.normalized * forceStrength);

                if (restrictHorizontalMovement)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
        }
    }
}
