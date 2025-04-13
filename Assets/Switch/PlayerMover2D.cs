using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover2D : MonoBehaviour
{
    public float speed = 2f;              // プレイヤーの移動速度
    public float moveDistance = 5f;       // 一方向に移動する距離

    private Vector3 startPosition;        // 移動開始地点
    private bool moving = false;          // 移動中フラグ
    private int direction = 1;            // 現在の移動方向（1 = 右, -1 = 左）

    void Update()
    {
        if (moving)
        {
            // 右または左に移動
            transform.position += Vector3.right * direction * speed * Time.deltaTime;

            // 指定した距離まで移動したら反転
            if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            {
                direction *= -1;                   // 向きを反転
                startPosition = transform.position;// 移動開始地点を更新
            }
        }
    }

    // 移動開始関数（外部から呼び出し）
    public void StartMoving()
    {
        if (!moving)
        {
            moving = true;
            startPosition = transform.position;
        }
    }
}