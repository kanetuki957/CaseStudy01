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
        }
        
            if (other.CompareTag("Player"))
            {
                // プレイヤーがゴールに触れたら、次のシーンを読み込む
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
