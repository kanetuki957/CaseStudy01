using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayEffect()
    {
        if (audioSource != null)
        {
            if (audioSource.clip == null)
            {
                Debug.LogWarning($"[SoundEffect] AudioClip Ç™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ: {gameObject.name}");
                return;
            }

            audioSource.Play();
        }
    }
}
