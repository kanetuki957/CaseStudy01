using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover2D : MonoBehaviour
{
    public float speed = 2f;              // 移動速度
    public float moveDistance = 5f;       // 一方向に移動する距離

    private Vector3 initialPosition;      // 初期位置
    private Vector3 startPosition;        // 1周期の開始位置
    private int direction = 1;            // 現在の移動方向（1 = 右, -1 = 左）

    private bool moving = false;          // 現在移動中かどうか

    void Start()
    {
        initialPosition = transform.position; // 初期位置を保存
    }

    void Update()
    {
        if (moving)
        {
            // 左右に移動
            transform.position += Vector3.right * direction * speed * Time.deltaTime;

            // 一定距離移動したら方向転換
            if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            {
                direction *= -1;
                startPosition = transform.position;
            }
        }
    }

    // クリックで呼ばれる切り替え処理
    public void ToggleMove()
    {
        if (!moving)
        {
            // 移動開始
            moving = true;
            startPosition = transform.position;
            direction = 1; // 初回は右へ
        }
        else
        {
            // 移動停止＆即座に初期位置へ戻す
            moving = false;
            transform.position = initialPosition;
        }
    }
}