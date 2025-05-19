using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// このスクリプトは、クリックされたオブジェクト（名前が "Switch"）に応じて
// ２体のプレイヤーオブジェクトの動作を同時に切り替えるための制御を行う。

public class ClickDetector_Multi : MonoBehaviour
{
    public GameObject player;  // プレイヤーオブジェクト（Inspector で指定）
    public GameObject player2; // プレイヤーオブジェクト２（Inspector で指定）

    void Update()
    {
        // マウスの左クリックを検出（毎フレーム）
        if (Input.GetMouseButtonDown(0))
        {
            // クリックされた画面上の座標をワールド座標に変換
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // クリックされた画面上の座標をワールド座標に変換
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                // クリックされたオブジェクトの名前が "Switch" かどうかを確認
                // 名前で判定することで、特定のオブジェクトにのみ反応させる
                if (hit.collider.gameObject.name == "Switch")
                {
                    // 2体のプレイヤーの動作を同時に切り替える
                    // ToggleMove() は、移動開始 ? 停止＋初期位置に戻る を切り替える
                    player.GetComponent<PlayerMover2D>().ToggleMove();
                    player2.GetComponent<PlayerMover2D>().ToggleMove();
                }
            }
        }
    }
}
