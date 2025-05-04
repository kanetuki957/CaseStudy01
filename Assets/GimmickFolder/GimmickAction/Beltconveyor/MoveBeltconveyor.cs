using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 指定のGameObjectを左に移動させる処理を管理するスクリプト
public class MoveBeltconveyor : MonoBehaviour
{
    // 左方向への移動速度
    public float moveSpeed = 2f;

    // このX座標より左に来たら移動を停止する
    public float stopXPosition = -10f;

    // 移動中かどうかのフラグ
    private bool isMoving = false;

    void Update()
    {
        // 移動フラグが立っている間は左に移動し続ける
        if (isMoving)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

            // 指定座標より左に出たら止める
            if (transform.position.x <= stopXPosition)
            {
                isMoving = false;
            }
        }
    }

    // 外部からこの関数を呼ぶことで、左向きにして移動を開始する
    public void StartMovingLeft()
    {
        // 右向きでも必ず左向きにする（スケール反転）
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;

        // 移動開始
        isMoving = true;
    }
}