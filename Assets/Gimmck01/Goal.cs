using UnityEngine;
using System.Collections.Generic;

public class Goal : MonoBehaviour
{
    private HashSet<GameObject> touchingPlayers = new HashSet<GameObject>();

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
