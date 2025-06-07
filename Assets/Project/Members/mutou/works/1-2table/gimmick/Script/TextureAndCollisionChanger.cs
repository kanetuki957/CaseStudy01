using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 指定のオブジェクトに近づくと、対象オブジェクトのスプライトを変更し、当たり判定を削除するクラス
public class TextureAndCollisionChanger : MonoBehaviour
{
    public GameObject triggerObject;           // プレイヤーが接触する対象
    public List<GameObject> targetObjects;     // スプライトと当たり判定を変更するオブジェクト
    public Sprite newSprite;                   // 差し替えるスプライト画像
    public float triggerRange = 0.5f;          // 発動判定となる距離

    private bool triggered = false;

    void Update()
    {
        // 指定距離内に triggerObject が入ったら1回だけ実行
        if (!triggered && triggerObject != null &&
            Vector3.Distance(transform.position, triggerObject.transform.position) <= triggerRange)
        {
            triggered = true;

            foreach (GameObject obj in targetObjects)
            {
                if (obj == null) continue;

                // スプライトを変更
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null && newSprite != null)
                {
                    sr.sprite = newSprite;
                }

                // 自身および子の Collider2D を全て削除（すり抜け可能にする）
                Collider2D[] allCols = obj.GetComponentsInChildren<Collider2D>();
                foreach (var col in allCols)
                {
                    Destroy(col);
                }
            }
        }
    }
}