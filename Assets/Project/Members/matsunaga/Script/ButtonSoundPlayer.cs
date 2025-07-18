using UnityEngine;

public class ButtonSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayButtonSound()
    {
        Debug.Log("PlayButtonSound ‚ªŒÄ‚Î‚ê‚Ü‚µ‚½");

        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("AudioSource.Play() Às");
        }
        else
        {
            Debug.LogWarning("AudioSource ‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
        }
    }
}
