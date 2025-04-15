using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetector2D : MonoBehaviour
{
    public GameObject player; // プレイヤーオブジェクト（Inspector で指定）

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // マウス位置をワールド座標に変換
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Raycastで２Ｄオブジェクトを判定
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                // 名前で判定（またはスクリプトで判定）
                if (hit.collider.gameObject.name == "Switch")
                {
                    player.GetComponent<PlayerMover2D>().ToggleMove();
                }
            }
        }
    }
}
