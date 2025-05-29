using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;      // 移動速度（単位: ユニット/秒）
    public float leftLimit = -5f; // 左端の位置（この座標に達すると右へ移動）
    public float rightLimit = 5f; // 右端の位置（この座標に達すると左へ移動）
    private int direction = 1;    // 移動方向（1: 右, -1: 左）

    public GameObject[] textureObjects; // プレイヤーのテクスチャ変更オブジェクト
    public Sprite[] textureStages;      // プレイヤーのテクスチャ一覧
    public float scaleMultiplier = 1.2f;// スケール倍率
    private int currentStage = 0;       // 現在のテクスチャ段階

    // プレイヤーのオブジェクト名
    public string playerObjectName;

    void Update()
    {
        // 敵を現在の移動方向に移動させる
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // 左端または右端に達したら移動方向を反転
        if (transform.position.x <= leftLimit)
        {
            direction = 1;  // 右へ移動
        }
        else if (transform.position.x >= rightLimit)
        {
            direction = -1; // 左へ移動
        }
    }

    // プレイヤーと衝突した際の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 衝突したオブジェクトなら処理を実行
        if (collision.gameObject.name == playerObjectName)
        {

            if (PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.ChangePlayerTexture(2);// 敵（ＨＰ）のテクスチャ変更
                ChangeEnemyTexture(2);                       // 敵（ＨＰ）のテクスチャ変更
            }

            direction = 1; // 敵の移動方向を右に固定
        }
    }

    // 敵側のテクスチャを指定回数変更する関数
    private void ChangeEnemyTexture(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            // テクスチャがまだ残っている場合のみ変更
            if (currentStage < textureObjects.Length)
            {
                GameObject obj = textureObjects[currentStage];

                if (obj != null)
                {
                    // 敵のスプライトレンダラーを取得し、テクスチャ変更
                    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.sprite = textureStages[currentStage];    // テクスチャを更新
                        obj.transform.localScale *= scaleMultiplier;// スケール拡大
                    }
                    currentStage++;
                }
            }
        }
    }
}