using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aitem : MonoBehaviour
{
    public List<string> collectedItems = new List<string>();
    public List<GameObject> targetObjects = new List<GameObject>(); // ���Ȃ�
    public Sprite openedSprite;
    public float effectRange = 3.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �A�C�e���擾
        if (other.CompareTag("Item"))
        {
            string itemName = other.gameObject.name;
            collectedItems.Add(itemName);
            Destroy(other.gameObject);
            Debug.Log(itemName + " ���擾�����I");
        }

        // �A�C�e���g�p�]�[���i���Ȃǁj
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
            Debug.Log(usedItem + " ���g�p�����I");

            foreach (GameObject obj in targetObjects)
            {
                if (obj != null && Vector3.Distance(transform.position, obj.transform.position) <= effectRange)
                {
                    // �X�v���C�g�ύX
                    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                    if (sr != null && openedSprite != null)
                    {
                        sr.sprite = openedSprite;
                    }

                    // �R���C�_�[���폜 or ������
                    Collider2D col = obj.GetComponent<Collider2D>();
                    if (col != null)
                    {
                        Destroy(col); // �� ���S�ɍ폜����Ȃ炱��
                        // col.enabled = false; // �� �����������������Ȃ炱��
                    }

                    Debug.Log(obj.name + " ���J�����I");
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