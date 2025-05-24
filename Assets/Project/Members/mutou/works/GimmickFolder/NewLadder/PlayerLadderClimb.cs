using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderClimb : MonoBehaviour
{
    public string ladderObjectName = "Ladder";
    public float climbSpeed = 2f;
    public float topY = 5f;      // 上端のY座標
    public float bottomY = 1f;   // 下端のY座標

    private Rigidbody2D rb;
    private GameObject currentLadder = null;
    private bool isOnLadder = false;
    private int climbDirection = 1; // 1 = 上, -1 = 下

    private PlayerMovePro moveScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveScript = GetComponent<PlayerMovePro>();
    }

    void Update()
    {
        if (isOnLadder)
        {
            // 横移動・ジャンプを無効化
            if (moveScript.enabled)
                moveScript.enabled = false;

            // 重力無効
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;

            // X座標をラダーに合わせる
            if (currentLadder != null)
            {
                transform.position = new Vector3(
                    currentLadder.transform.position.x,
                    transform.position.y,
                    transform.position.z
                );
            }

            // 上下移動（transformで制御）
            transform.position += new Vector3(0, climbSpeed * climbDirection * Time.deltaTime, 0);

            // 到達したら反転
            if (climbDirection == 1 && transform.position.y >= topY)
                climbDirection = -1;
            else if (climbDirection == -1 && transform.position.y <= bottomY)
                climbDirection = 1;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == ladderObjectName)
        {
            currentLadder = other.gameObject;
            isOnLadder = true;

            // 最初だけX座標を合わせる（ぴたっと吸い付く演出）
            transform.position = new Vector3(
                currentLadder.transform.position.x,
                transform.position.y,
                transform.position.z
            );
        }
    }

    // 一度でもはしごに入ったら降りても登り続ける → Exit無視
    void OnTriggerExit2D(Collider2D other)
    {
        // 処理しない（仕様通り）
    }
}