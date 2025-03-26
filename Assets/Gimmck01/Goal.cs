using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UIを使うために必要

public class Goal : MonoBehaviour
{
    public string main_goal; // 遷移先のシーン名をInspectorで指定

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // プレイヤーが触れたら
        {
            SceneManager.LoadScene(main_goal); // 指定したシーンへ移動
        }
    }
}
