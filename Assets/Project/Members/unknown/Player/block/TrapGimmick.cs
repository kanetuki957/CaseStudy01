using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGimmick : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<PlayerController>() != null) // PlayerController を持っているかチェック
        {

            AnimationController animationController = other.GetComponent<AnimationController>();

            animationController.TrapBool();

            Destroy(gameObject); // 自分を削除

        }

    }
}