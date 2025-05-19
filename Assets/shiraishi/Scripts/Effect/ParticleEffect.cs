using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public ParticleSystem targetParticleSystem;
    void Awake()
    {
        if (targetParticleSystem == null)
            targetParticleSystem = GetComponent<ParticleSystem>();
    }

    public void PlayEffect()
    {
        targetParticleSystem?.Play();
    }
}
