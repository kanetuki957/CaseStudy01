using UnityEngine;

public class ForceZone : MonoBehaviour
{
    [Header("����������i���K������܂��j")]
    public Vector2 forceDirection = Vector2.up;

    [Header("������͂̋���")]
    public float forceStrength = 10f;

    [Header("���ړ��𐧌����邩")]
    public bool restrictHorizontalMovement = false;

    [SerializeField] private EffectManager effectManager;

    private void Start()
    {
        // �q���� EffectManager �������ŒT���i���ݒ莞�̂݁j
        if (effectManager == null)
        {
            effectManager = GetComponentInChildren<EffectManager>();
            if (effectManager == null)
            {
                Debug.LogWarning($"[DisappearBlock] �q�I�u�W�F�N�g�� EffectManager ��������܂���: {gameObject.name}");
            }
        }
    }

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
