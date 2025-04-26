using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 【ハシゴを登ったあとに、ターゲットオブジェクトへ移動し、触れたら初期位置へゆっくり戻るスクリプト】
public class MoveToTargetAfterClimb : MonoBehaviour
{
    public GameObject targetObject;     // 向かっていく目標オブジェクト
    public float moveSpeed = 2f;        // ターゲットまでの移動スピード
    public float returnSpeed = 1f;      // 初期位置に戻るときのスピード

    private bool canMove = false;       // 移動を開始していいかどうか
    private bool returnToStart = false; // 初期位置に戻るモードかどうか
    private Vector3 startPosition;      // 最初にいた位置（初期位置）

    void Start()
    {
        startPosition = transform.position; // 最初の座標を保存しておく
    }

    void Update()
    {
        if (canMove && !returnToStart)
        {
            // ターゲットに向かって移動する
            MoveTowardsTarget(targetObject.transform.position);
        }
        else if (returnToStart)
        {
            // 初期位置に戻る
            MoveTowardsTarget(startPosition);
        }
    }

    // 外部から呼び出して「移動を開始する」ための関数
    public void StartMoving()
    {
        canMove = true; // 移動を開始OKにする
    }

    // 指定したターゲットへ向かって移動する処理
    void MoveTowardsTarget(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position).normalized;  // ターゲットへの方向を計算
        float speed = returnToStart ? returnSpeed : moveSpeed;      // 戻るときはreturnSpeed、それ以外はmoveSpeedを使う

        transform.position += dir * speed * Time.deltaTime;         // ターゲット方向へ移動する

        // ターゲットに近づいたら停止する
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            if (!returnToStart)
            {
                // ターゲットに到着したとき → 移動ストップ
                canMove = false;
            }
            else
            {
                // 初期位置に戻ったとき → 移動ストップ
                returnToStart = false;
            }
        }
    }

    // ターゲットに触れたときの処理（OnTriggerEnter2D）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == targetObject)
        {
            // もしターゲットオブジェクトに触れたら、初期位置に戻るモードに切り替える
            returnToStart = true;
            canMove = false; // いったん移動停止（戻る処理はUpdate側で動く）
        }
    }
}