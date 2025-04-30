using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UIを使うために必要

public class Goal : MonoBehaviour
{
    public string main_goal; // 遷移先のシーン名をInspectorで指定
    private GameObject player; // プレイヤーオブジェクトを格納する変数

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // プレイヤーオブジェクトを取得
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // プレイヤーが触れたら
        {
            if (GameObject.Find("key")) // シーン内にキーがある場合
            {
                if (player.GetComponent<aitem>().collectedItems.Contains("key"))    // プレイヤーが取得したアイテム内にキーがあるなら
                {
                    SceneManager.LoadScene(main_goal); // 指定したシーンへ移動
                }
            }
            else
                SceneManager.LoadScene(main_goal); // 指定したシーンへ移動
        }
    }
}
