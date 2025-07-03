using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private HashSet<GameObject> touchingPlayers = new HashSet<GameObject>();


    [Header("ƒS[ƒ‹‚µ‚½‚ÌSE")]
    [SerializeField] private AudioClip trapHitSE;        // •Ç‚É‰ƒqƒbƒg‚µ‚½‚Ì SE
    [SerializeField][Range(0f, 1f)] private float seVolume = 1f;



    public bool IsPlayerTouching(GameObject player)
    {
        return touchingPlayers.Contains(player);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            touchingPlayers.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            touchingPlayers.Remove(other.gameObject);
        }
    }

}
