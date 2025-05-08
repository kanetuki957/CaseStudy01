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
                Debug.LogWarning($"[SoundEffect] AudioClip ���ݒ肳��Ă��܂���: {gameObject.name}");
                return;
            }

            audioSource.Play();
        }
    }
}
