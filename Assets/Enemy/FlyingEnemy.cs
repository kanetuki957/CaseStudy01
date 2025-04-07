using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float moveSpeed = 1f;       // 左方向への移動速度
    public float floatSpeed = 2f;      // 上下の揺れの速さ
    public float floatHeight = 2f;     // 上下の振れ幅

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // 左に少しずつ移動
        float moveX = transform.position.x - moveSpeed * Time.deltaTime;

        // 上下にゆらゆら
        float moveY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // 新しい位置に移動
        transform.position = new Vector3(moveX, moveY, transform.position.z);
    }
}
