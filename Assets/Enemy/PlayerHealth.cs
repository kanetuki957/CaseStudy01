using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // シングルトンパターン（他のスクリプトから `GameDirector.Instance` でアクセスできる）
    public static PlayerHealth Instance;

    [SerializeField]
    private GameObject[] textureObjects; // 順番に変更するオブジェクトの配列

    [SerializeField]
    private Sprite[] textureStages; // 変更するテクスチャの配列
    private int currentStage = 0;   // 現在のオブジェクトインデックス（どのオブジェクトを変更するか管理）
    public float scaleMultiplier;   // スケールの拡大率（変更後のサイズを決める）

    void Awake()
    {
        // シングルトンの設定（既に存在する場合は削除）
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // プレイヤー側のテクスチャを指定回数変更する関数
    public void ChangePlayerTexture(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            // テクスチャがまだ残っている場合のみ変更
            if (currentStage < textureObjects.Length)
            {
                GameObject obj = textureObjects[currentStage];

                if (obj != null)
                {
                    // スプライトレンダラーを取得し、テクスチャ変更
                    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.sprite = textureStages[currentStage];    // テクスチャを更新
                        obj.transform.localScale *= scaleMultiplier;// スケールを拡大
                    }
                    currentStage++;// 次の段階へ
                }
            }
        }
    }
}