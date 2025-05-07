using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aitem : MonoBehaviour
{
    public List<string> collectedItems = new List<string>();
    public List<GameObject> desObject = new List<GameObject>();
    public float desRange = 3.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // アイテム取得ゾーン
        if (other.CompareTag("Item"))
        {
            string itemName = other.gameObject.name;
            collectedItems.Add(itemName);
            Destroy(other.gameObject);
            Debug.Log(itemName + " を取得した！");
        }

        // アイテム使用ゾーン
        if (other.CompareTag("UseZone"))
        {
            UseItem();
        }
    }

    void UseItem()
    {
        if (collectedItems.Count > 0)
        {
            string usedItem = collectedItems[0];
            collectedItems.RemoveAt(0);
            Debug.Log(usedItem + " を使用した！");

            foreach (GameObject obj in desObject)
            {
                if (obj != null && Vector3.Distance(transform.position, obj.transform.position) <= desRange)
                {
                    Destroy(obj);
                    Debug.Log(obj.name + " を削除");
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
