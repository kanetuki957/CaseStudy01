using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 指定のオブジェクトに近づくと、対象オブジェクトのスプライトを変更し、当たり判定を削除するクラス
public class TextureAndCollisionChanger : MonoBehaviour
{
    public GameObject triggerObject;           // スイッチ（触れるとドアが開く）
    public List<GameObject> targetObjects;     // ドア（スプライト変更＆コライダー削除対象）
    public Sprite newSprite;                   // 開いたドアのスプライト

    private bool triggered = false;

    void Start()
    {
        // スタート時にすべてのドアのColliderを有効化（子も含めて）
        foreach (GameObject obj in targetObjects)
        {
            if (obj == null) continue;

            Collider2D[] colliders = obj.GetComponentsInChildren<Collider2D>();
            foreach (var col in colliders)
            {
                col.enabled = true;
            }
        }

        Debug.Log("[TextureAndCollisionChanger] 全ドアのコライダーをONにしました");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 発動済みならスキップ
        if (triggered) return;

        // プレイヤー自身が triggerObject に触れたかどうか
        if (triggerObject != null &&
            other.gameObject == triggerObject)
        {
            triggered = true;

            foreach (GameObject obj in targetObjects)
            {
                if (obj == null) continue;

                // スプライト変更（子も含めて探す）
                SpriteRenderer sr = obj.GetComponentInChildren<SpriteRenderer>();
                if (sr != null && newSprite != null)
                {
                    sr.sprite = newSprite;
                }

                // コライダー削除（子も含めて）
                Collider2D[] colliders = obj.GetComponentsInChildren<Collider2D>();
                foreach (var col in colliders)
                {
                    Destroy(col);
                }
            }
        }
    }
}