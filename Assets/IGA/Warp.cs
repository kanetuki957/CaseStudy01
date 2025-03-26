using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField] private Vector3 warpPosition; // ワープ先の位置

    private bool isPlayerInside = false;  // プレイヤーが土管の中にいるかどうか
    private GameObject player;          // プレイヤーのオブジェクト保持

    // プレイヤーがワープに触れているとき
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true; // ワープしている状態にする
            player = other.gameObject; // プレイヤーのオブジェクトを取得
        }
    }

    // プレイヤーがワープゾーンから出たとき
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false; // ワープ不可状態にする
            player = null; // プレイヤー情報をリセット
        }
    }

    // 毎フレームチェック
    private void Update()
    {
        // プレイヤーがゾーン内にいて、ワープキーが押されたらワープ
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Fキーが押された！");
        }

        if (isPlayerInside && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("ワープ処理が呼ばれた！");
            WarpPlayer();
        }
    }

    // プレイヤーをワープさせる
    private void WarpPlayer()
    {
        if (player != null)
        {
            Debug.Log("ワープ開始！現在の位置: " + player.transform.position);
            player.transform.position = warpPosition; // ワープ
            Debug.Log("ワープ完了！新しい位置: " + player.transform.position);
            player.transform.position = warpPosition; // プレイヤーの位置をワープ先に変更
        }
    }
}
