using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAndCollisionChanger : MonoBehaviour
{
    public GameObject triggerObject;           // 接触対象（例：スイッチ）
    public List<GameObject> targetObjects;     // 扉など（Colliderをまとめて処理）
    public Sprite newSprite;
    public float triggerRange = 0.5f;

    private bool triggered = false;

    void Update()
    {
        if (!triggered && triggerObject != null &&
            Vector3.Distance(transform.position, triggerObject.transform.position) <= triggerRange)
        {
            triggered = true;

            foreach (GameObject obj in targetObjects)
            {
                if (obj == null) continue;

                // SpriteRenderer の差し替え
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null && newSprite != null)
                {
                    sr.sprite = newSprite;
                }

                // 自身と子を含むすべての Collider2D を削除
                Collider2D[] allCols = obj.GetComponentsInChildren<Collider2D>();
                foreach (var col in allCols)
                {
                    Destroy(col);
                }
            }
        }
    }
}
