using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class copycount : MonoBehaviour
{
    public GameObject targetObject; // Inspector�őΏۃI�u�W�F�N�g��ݒ�
    public int copynumber;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null) // PlayerController �������Ă��邩�`�F�b�N
        {
            CreateBlock createBlock = targetObject.GetComponent<CreateBlock>();

            createBlock.PlusCopy(copynumber);

            Destroy(gameObject); // �������폜

        }
       
    }

}
