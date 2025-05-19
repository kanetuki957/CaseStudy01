using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover2D : MonoBehaviour
{
    public float speed = 2f;              // プレイヤーの移動速度
    public float moveDistance = 5f;       // 1回の往復で移動する距離
    public int initialDirection = 1;      // 初期方向（1 = 右, -1 = 左）

    private Vector3 initialPosition;      // 初期位置（戻るときの基準）
    private Vector3 startPosition;        // 1往復の開始地点
    private int direction;                // 現在の移動方向
    private bool moving = false;          // 現在移動中かどうか

    void Start()
    {
        // 初期位置を保存（クリックで戻すため）
        initialPosition = transform.position;

        // 初期の移動方向を設定
        direction = initialDirection;
    }

    void Update()
    {
        if (moving)
        {
            // 指定方向に移動（毎フレーム）
            transform.position += Vector3.right * direction * speed * Time.deltaTime;

            // 指定距離以上移動したら反転（往復運動）
            if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            {
                direction *= -1;                   // 移動方向を反転
                startPosition = transform.position;// 新たな出発点に更新
            }
        }
    }

    // 外部から呼び出して移動の開始/停止を切り替える関数
    public void ToggleMove()
    {
        if (!moving)
        {
            // 移動を開始
            moving = true;

            // 初期方向で再開する（クリック毎に初期化）
            direction = initialDirection;

            // 現在位置を出発点として記録
            startPosition = transform.position;
        }
        else
        {
            // 移動を停止し、初期位置へ即座に戻す
            moving = false;
            transform.position = initialPosition;
        }
    }
}