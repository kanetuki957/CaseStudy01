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
    private Vector3 startPostion;
    private bool isTrapReset;
    private bool isgoal;
    private bool isgoalPerformance;
    private Rigidbody rb;
    private Collider myCollider;
    private SpriteRenderer spriteRenderer;
    private AnimationController animationController;
    private float moveX;
    private Vector3 direction;

    void Start()
    {
        startPostion = transform.position;  // 初期位置を記録
        animationController = target.GetComponent<AnimationController>();

        myCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < ignoredPlayers.Length; i++)
        {
            Collider playerCollider = ignoredPlayers[i].GetComponent<Collider>();
            if (playerCollider != null)
            {
                Physics.IgnoreCollision(myCollider, playerCollider);
            }
        }
    }

    void FixedUpdate()
    {
        isgoal = animationController.GoalButton;
        if (!isgoal)
        {
            direction = (target.position - rb.position).normalized;
        }
        float distance = Vector3.Distance(target.position, rb.position);
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
            if (distance > 3.0f)
            {
                transform.position = target.position + new Vector3(-0.5f, 0, 0);  // 初期位置へ戻す
            }
        }

      
        if (isgoalPerformance)
        {

            float directionX = 0;
            if(directionX == 0)
            {
                directionX = direction.x;
            }
            if (directionX > 0)
            {
                moveX = 1.0f; // 右向き
            }
            else if(directionX < 0)
            {
                moveX = -1.0f;
            }
            Vector3 move = new Vector3(moveX, 0, 0) * speed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
        else
        {
            // 一定の距離より近い場合は移動しない
            if (distance > desiredDistance)
            {
                rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
            }
        }
     
    }
    void RestartButton()
    {
       
    }
}
