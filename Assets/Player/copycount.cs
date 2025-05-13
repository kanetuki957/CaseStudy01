using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class copycount : MonoBehaviour
{
    public GameObject targetObject; // Inspectorで対象オブジェクトを設定
    public int copynumber;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null) // PlayerController を持っているかチェック
        {
            CreateBlock createBlock = targetObject.GetComponent<CreateBlock>();

            createBlock.PlusCopy(copynumber);

            Destroy(gameObject); // 自分を削除

        }
       
    }

}
