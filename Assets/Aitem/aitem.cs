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
        // �A�C�e���擾�]�[��
        if (other.CompareTag("Item"))
        {
            string itemName = other.gameObject.name;
            collectedItems.Add(itemName);
            Destroy(other.gameObject);
            Debug.Log(itemName + " ���擾�����I");
        }

        // �A�C�e���g�p�]�[��
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
            Debug.Log(usedItem + " ���g�p�����I");

            foreach (GameObject obj in desObject)
            {
                if (obj != null && Vector3.Distance(transform.position, obj.transform.position) <= desRange)
                {
                    Destroy(obj);
                    Debug.Log(obj.name + " ���폜");
                    break;
                }
            }
        }
        else
        {
            Debug.Log("�A�C�e��������܂���I");
        }
    }
}
