using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 【キャラクターが自動でハシゴへ向かって移動して、登り降りを１回だけ行うスクリプト】
public class AutoClimbApproacher : MonoBehaviour
{
    public GameObject ladderTarget;    // 登りに向かうハシゴのターゲット
    public float moveSpeed = 2f;       // 左へ移動するとき・登るときのスピード
    public float snapDistance = 1f;    // ハシゴに吸い付く距離（これより近づいたら吸着する）
    public float topY = 5f;            // ハシゴを登りきる高さ（目標Y座標）
    public float bottomY = 0f;         // ハシゴを下りきる高さ（目標Y座標）
    public float climbOffsetX = 0f;    // ハシゴとのX座標の補正（ずらしたい場合）

    private Rigidbody2D rb;            // Rigidbody2Dコンポーネントを入れておく変数

    // キャラクターの現在の行動ステート（何をしているか）
    private enum State { MoveToLadder, SnapToLadder, ClimbUp, ClimbDown }
    private State currentState = State.MoveToLadder; // 最初はハシゴに向かうところからスタート

    private bool finishedClimbing = false; // ハシゴを登り降りした後かどうかを記録するフラグ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2Dを取得しておく
    }

    void Update()
    {
        // もし登り降りが終わってたら、もう何もしない
        if (finishedClimbing) return;

        // 現在のステート（状態）によってやることを切り替える
        switch (currentState)
        {
            case State.MoveToLadder:
                MoveLeft();                   // 左に移動する
                CheckSnapDistance();          // ハシゴに近づいたかチェックする
                break;

            case State.SnapToLadder:
                SnapToLadder();               // ハシゴにぴったり吸着する
                currentState = State.ClimbUp; // 次は登り始めるステートに切り替える
                break;

            case State.ClimbUp:
                Climb(1);                     // 上に向かって登る
                break;

            case State.ClimbDown:
                Climb(-1);                    // 下に向かって降りる
                break;
        }
    }

    // 左方向に移動する関数
    void MoveLeft()
    {
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); // 左にmoveSpeedの速さで進む
    }

    // ハシゴに近づいたかどうか判定する関数
    void CheckSnapDistance()
    {
        float dist = Mathf.Abs(
            transform.position.x - ladderTarget.transform.position.x); // ハシゴとの横距離を計算

        if (dist <= snapDistance) // 許容範囲内に入ったら
        {
            currentState = State.SnapToLadder; // ステートを吸着モードに変更
        }
    }

    // ハシゴにぴったりX座標を合わせる関数
    void SnapToLadder()
    {
        // 移動を止めて、X位置をぴったり合わせる
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(
            ladderTarget.transform.position.x + climbOffsetX,
            transform.position.y,
            transform.position.z
        );
    }

    // ハシゴを登ったり降りたりする関数
    void Climb(int direction)
    {
        rb.velocity = new Vector2(0, moveSpeed * direction); // 上下方向に進む（direction=1なら上、-1なら下）

        // 登りきったら
        if (direction == 1 && transform.position.y >= topY)
        {
            rb.velocity = Vector2.zero;     // 動きを止める
            transform.position = new Vector3(
                transform.position.x,
                topY,
                transform.position.z);      // 正確な位置に合わせる

            currentState = State.ClimbDown; // 次は下りステートに切り替え
        }

        // 下りきったら
        else if (direction == -1 && transform.position.y <= bottomY)
        {
            rb.velocity = Vector2.zero; // 動きを止める
            transform.position = new Vector3(
                transform.position.x,
                bottomY,
                transform.position.z);  // 正確な位置に合わせる

            finishedClimbing = true;    // 登り降りが完了したとマークする

            // MoveToTargetAfterClimbスクリプトを探して、次のターゲットに向けて移動を開始させる
            FindObjectOfType<MoveToTargetAfterClimb>().StartMoving();
        }
    }
}