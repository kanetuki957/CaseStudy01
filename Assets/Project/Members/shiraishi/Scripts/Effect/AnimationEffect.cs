using UnityEngine;

public class AnimationEffect : MonoBehaviour
{
    public Animator animator;
    public string triggerName = "Play";

    public void PlayEffect()
    {
        if (animator != null && !string.IsNullOrEmpty(triggerName))
        {
            animator.SetTrigger(triggerName);
        }
    }
}
