using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targeteffect : MonoBehaviour
{
    public bool isInside = false; // �͈͓����ǂ������Ǘ�����ϐ�

    private void Update()
    {
        if (isInside == true)
        {
            Destroy(gameObject);
        }
    }

}
