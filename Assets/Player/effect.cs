using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class effect : MonoBehaviour
{
    public float checkRadius = 1f; // 判定する範囲の半径

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            targeteffect target = hitColliders[i].GetComponent<targeteffect>();
            if (target != null)
            {
                target.isInside = true; // 範囲内のオブジェクトのisInsideをtrueにする
            }
        }
    }
}