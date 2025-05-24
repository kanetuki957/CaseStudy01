using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 【キャラクターが特定オブジェクトに触れたら、指定したオブジェクト群を上下反転させるスクリプト】
public class FilpOnContact : MonoBehaviour
{
    public GameObject targetObject;           // ぶつかる対象
    public List<GameObject> objectsToReverse; // 反転させたいオブジェクトリスト

    private bool hasReversed = false;         // 1回だけ反転するためのフラグ

    // 他のオブジェクトとぶつかったときに呼ばれる関数
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // まだ反転していなくて、ターゲットオブジェクトにぶつかったら
        if (!hasReversed && collision.gameObject == targetObject)
        {
            foreach (GameObject obj in objectsToReverse)
            {
                if (obj != null)
                {
                    Vector3 scale = obj.transform.localScale; // 現在のスケールを取得
                    scale.y *= -1;                            // 上下だけ反転する
                    obj.transform.localScale = scale;         // 新しいスケールを適用
                }
            }

            hasReversed = true; // もう反転済みだとマークする（2回目以降は無視する）
        }
    }
}
