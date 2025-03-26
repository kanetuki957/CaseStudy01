using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAutoMove : MonoBehaviour
{
    public float speed = 3f;      // 移動速度
    public float leftLimit = -5f; // 左端の位置
    public float rightLimit = 5f; // 右端の位置
    private int direction = 1;    // 移動方向（1: 右, -1: 左）

    void Update()
    {
        // 敵を移動させる
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // 左端または右端に達したら移動方向を変える
        if (transform.position.x <= leftLimit)
        {
            direction = 1; // 右に移動
        }
        else if (transform.position.x >= rightLimit)
        {
            direction = -1; // 左に移動
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameDirector.Instance != null)
            {
                //ＨＰを「３秒間かけて」0.3減らす
                GameDirector.Instance.DecreaseHealth(0.3f, 3f);
            }

            direction = 1;
        }
    }

}
