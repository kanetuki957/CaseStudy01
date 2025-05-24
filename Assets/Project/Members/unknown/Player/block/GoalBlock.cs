using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<PlayerController>() != null) // PlayerController を持っているかチェック
        {

            AnimationController animationController = other.GetComponent<AnimationController>();

            animationController.GoalBool();

            Destroy(gameObject); // 自分を削除

        }

    }
}
