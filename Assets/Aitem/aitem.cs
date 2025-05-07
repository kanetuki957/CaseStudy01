using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aitem : MonoBehaviour
{
    public List<string> collectedItems = new List<string>();
    public List<GameObject> targetObjects = new List<GameObject>(); // 扉など
    public Sprite openedSprite;
    public float effectRange = 3.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // アイテム取得
        if (other.CompareTag("Item"))
        {
            string itemName = other.gameObject.name;
            collectedItems.Add(itemName);
            Destroy(other.gameObject);
            Debug.Log(itemName + " を取得した！");
        }

        // アイテム使用ゾーン（扉など）
        if (other.CompareTag("UseZone"))
        {
            UseItem(other.gameObject);
        }
    }

    void UseItem(GameObject useZoneObject)
    {
        if (collectedItems.Count > 0)
        {
            string usedItem = collectedItems[0];
            collectedItems.RemoveAt(0);
            Debug.Log(usedItem + " を使用した！");

            foreach (GameObject obj in targetObjects)
            {
                if (obj != null && Vector3.Distance(transform.position, obj.transform.position) <= effectRange)
                {
                    // スプライト変更
                    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                    if (sr != null && openedSprite != null)
                    {
                        sr.sprite = openedSprite;
                    }

                    // コライダーを削除 or 無効化
                    Collider2D col = obj.GetComponent<Collider2D>();
                    if (col != null)
                    {
                        Destroy(col); // ← 完全に削除するならこれ
                        // col.enabled = false; // ← 無効化したいだけならこれ
                    }

                    Debug.Log(obj.name + " を開いた！");
                    break;
                }
            }
        }
        else
        {
            Debug.Log("アイテムがありません！");
        }
    }
}