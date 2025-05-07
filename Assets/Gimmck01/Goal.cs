using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
            Debug.Log("Player reached goal!");

            // シーン名で遷移する例
            SceneManager.LoadScene("main_goal"); // 実際のシーン名に置き換える
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
