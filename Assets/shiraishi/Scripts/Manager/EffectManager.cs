using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public AnimationEffect animationEffect;
    public ParticleEffect particleEffect;
    public SoundEffect soundEffect;

    public bool deactivateAfterPlay = false;

    public void PlayAllEffects()
    {
        animationEffect?.PlayEffect();
        particleEffect?.PlayEffect();
        soundEffect?.PlayEffect();

        if (deactivateAfterPlay)
            StartCoroutine(DeactivateAfterDelay(1f));
    }

    private System.Collections.IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
