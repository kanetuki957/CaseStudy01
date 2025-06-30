using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが特定のはしごに接近したとき、
/// 自動で上または下に移動する処理を行うスクリプト。
/// 
/// ・はしご「LadderUp」：上に登る専用
/// ・はしご「LadderDown」：下に降りる専用
/// 
/// ・登り終える（topY）または下り終える（bottomY）と、元の横移動（PlayerMovePro）が再開される。
/// ・はしご中は重力を無効にし、X座標をラダーに吸着する。
/// </summary>
public class PlayerLadder2 : MonoBehaviour
{
    // 登り用はしごのオブジェクト名
    public string[] ladderUpObjectName = { "LadderUp" };

    // 下り用はしごのオブジェクト名
    public string[] ladderDownObjectName = { "LadderDown" };

    // 登り・下りのスピード
    public float climbSpeed = 2f;

    // 登り切る高さ（Y座標）
    public float topY = 5f;

    // 下り切る高さ（Y座標）
    public float bottomY = 1f;

    private Rigidbody2D rb;
    private GameObject currentLadder = null; // 現在触れているはしご
    public bool isOnLadder = false;         // はしごに乗っているかどうか
    private int climbDirection = 0;          // 1 = 上に登る, -1 = 下に降りる

    private float originalGravityScale;      // 登り中に重力を止めるため、元の値を保存
    private PlayerMove moveScript;        // 横移動用スクリプト

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveScript = GetComponent<PlayerMove>();

        // gravityScale を記憶しておく（復帰時に使用）
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        // はしごに乗っているときだけ動作する
        if (isOnLadder)
        {
            // 横移動を一時停止
            if (moveScript.enabled)
                moveScript.enabled = false;

            // 横移動を一時停止
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;

            // はしごのX座標に吸着（ピタッと合わせる）
            if (currentLadder != null)
            {
                transform.position = new Vector3(
                    currentLadder.transform.position.x,
                    transform.position.y,
                    transform.position.z
                );
            }

            // 上または下に移動（登り降り）
            transform.position += new Vector3(0, climbSpeed * climbDirection * Time.deltaTime, 0);

            // 上まで登ったら止める
            if (climbDirection == 1 && transform.position.y >= topY)
                FinishClimb(topY);
            // 下まで降りたら止める
            else if (climbDirection == -1 && transform.position.y <= bottomY)
                FinishClimb(bottomY);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // すでに乗っている場合は何もしない（1回限り）
        if (isOnLadder) return;

        // ① 登りラダー判定
        if (System.Array.Exists(ladderUpObjectName, name => name == other.name))
        {
            climbDirection = 1;
        }
        // ② 下りラダー判定
        else if (System.Array.Exists(ladderDownObjectName, name => name == other.name))
        {
            climbDirection = -1;
        }
        // ③ どちらでもなければ無視
        else
        {
            return;
        }
        //for(int i = 0; i < ladderUpObjectName.Length; i++)
        //{
        //    // 登りはしごに触れた
        //    if (other.name == ladderUpObjectName[i])
        //    {
        //        climbDirection = 1;
        //    }
        //    // 下りはしごに触れた
        //    else if (other.name == ladderDownObjectName[i])
        //    {
        //        climbDirection = -1;
        //    }
        //    else
        //    {
        //        return; // それ以外のオブジェクトは無視
        //    }

        //}


        currentLadder = other.gameObject;
        isOnLadder = true;

        // X座標の差が近ければはしごに吸着する（吸い付き感の調整）
        float distanceX = Mathf.Abs(transform.position.x - currentLadder.transform.position.x);
        float snapThreshold = 0.1f;

        if (distanceX < snapThreshold)
        {
            transform.position = new Vector3(
                currentLadder.transform.position.x,
                transform.position.y,
                transform.position.z
            );
        }
    }

    // 登りまたは下りの移動が完了したときに呼ばれる
    void FinishClimb(float fixedY)
    {
        // Y座標を正確に合わせる
        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
        rb.velocity = Vector2.zero;

        // 重力と横移動を復帰
        rb.gravityScale = originalGravityScale;
        moveScript.enabled = true;

        // 状態をリセット（次のはしごのために）
        isOnLadder = false;
        currentLadder = null;
        climbDirection = 0;
         }
}