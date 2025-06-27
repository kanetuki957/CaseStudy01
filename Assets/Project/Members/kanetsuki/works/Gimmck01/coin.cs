using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    public AudioClip pickupSE;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Œ©‚½–Ú‚ğÁ‚·•“–‚½‚è”»’èƒIƒt
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            if (col != null) col.enabled = false;

            // SEÄ¶
            audioSource.PlayOneShot(pickupSE);

            // SE‚ªI‚í‚Á‚½‚çíœ
            Destroy(gameObject, pickupSE.length);
        }
    }
}
