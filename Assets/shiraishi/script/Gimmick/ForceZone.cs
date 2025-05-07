using UnityEngine;

public class ForceZone : MonoBehaviour
{
    [Header("‰Á‚¦‚é•ûŒüi³‹K‰»‚³‚ê‚Ü‚·j")]
    public Vector2 forceDirection = Vector2.up;

    [Header("‰Á‚¦‚é—Í‚Ì‹­‚³")]
    public float forceStrength = 10f;

    [Header("‰¡ˆÚ“®‚ğ§ŒÀ‚·‚é‚©")]
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
