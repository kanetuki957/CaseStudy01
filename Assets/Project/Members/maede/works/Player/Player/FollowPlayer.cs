using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float desiredDistance = 2f; // 一定の距離を保つ
    public GameObject[] ignoredPlayers; // 通り抜けたいプレイヤーのリスト
    public float rayDistance = 0.0f;

    private bool isgoal;
    private bool isgoalPerformance;
    private bool button = false;
    private Rigidbody2D rb;
    private Collider2D myCollider;
    private SpriteRenderer spriteRenderer;
    private AnimationController animationController;
    private float moveX;
    private Vector2 direction;
    private bool jump = false;

    void Start()
    {
        
        animationController = target.GetComponent<AnimationController>();
        myCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < ignoredPlayers.Length; i++)
        {
            Collider2D playerCollider = ignoredPlayers[i].GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, myCollider);
            }
        }
    }


    void FixedUpdate()
    {
        button = animationController.Button; 
        jump = animationController.skyLeapBool;
        isgoal = animationController.GoalButton;
        if (!isgoal)
        {
            direction = ((Vector2)target.position - rb.position).normalized;
        }
        float distance = Vector2.Distance((Vector2)target.position, rb.position);
        // スプライトの向きを反転

        isgoalPerformance = animationController.goalPerformance;
        if (!isgoalPerformance)
        {
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // 右向き
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true; // 左向き
            }
            if (distance > 4.0f)
            {
                transform.position = target.position + new Vector3(-1.3f, 0, 0);  // 初期位置へ戻す
            }
        }
     
        if (button)
        {

            if (isgoalPerformance)
            {
                // ゴール演出時：通常の動き（X方向のみ）
                moveX = direction.x > 0 ? 1f : -1f;

                Vector2 velocity = rb.velocity;
                velocity.x = moveX * speed;
                rb.velocity = velocity;

            }
            else
            {
                if (distance > desiredDistance)
                {
                    Vector2 targetVelocity = new Vector2(direction.x * speed, rb.velocity.y);

                    if (!jump)
                    {
                        targetVelocity.y = direction.y * speed;
                    }

                    // 今の速度から目標速度へ徐々に近づける（滑らかになる）
                    rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, 0.5f);  // 0.1fは調整可能
                }
                else
                {
                    Vector2 targetVelocity = new Vector2(0, rb.velocity.y);
                    rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, 0.5f);
                }

                //if (distance > desiredDistance)
                //{
                //    Vector2 velocity = rb.velocity;
                //    velocity.x = direction.x * speed; // X軸のみ追従
                //    if(!jump)
                //    {
                //        velocity.y = direction.y * speed;
                //    }
                //    rb.velocity = new Vector2(velocity.x, velocity.y); // Y軸はそのまま重力に任せる
                //}
                //else
                //{
                //    // 距離が近いときは横移動止めるが、落下などは許容
                //    rb.velocity = new Vector2(0, rb.velocity.y);
                //}
            }
        }

    }
    void SkyLeap()
    {
        if (rb.velocity.y > 0) // 正の速度ならジャンプ中
        {
            jump = true;
        }
        else if (rb.velocity.y <= 0) // 負の速度なら落下中
        {
            jump = false;
        }

    }
}