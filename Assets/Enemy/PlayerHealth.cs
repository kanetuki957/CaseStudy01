using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // シングルトンパターン（他のスクリプトから `GameDirector.Instance` でアクセスできる）
    public static PlayerHealth Instance;

    [SerializeField]
    private GameObject[] textureObjects; // **順番に変更するオブジェクトの配列**

    [SerializeField]
    private Sprite[] textureStages; // **変更するテクスチャの配列**

    private int currentStage = 0; // **現在のオブジェクトインデックス（どのオブジェクトを変更するか管理）**
    public float scaleMultiplier; // **スケールの拡大率（変更後のサイズを決める）**

    void Awake()
    {
        // **シングルトンの設定（既に存在する場合は削除）**
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // **1回の衝突ごとに1つのオブジェクトのテクスチャを変更する関数**
    public void ChangeTexture()
    {
        // **まだ変更できるオブジェクトがある場合のみ実行**
        if (currentStage < textureObjects.Length)
        {
            GameObject obj = textureObjects[currentStage]; // **次に変更するオブジェクトを取得**

            if (obj != null)
            {
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = textureStages[currentStage]; // **対応するテクスチャに変更**

                    // **スケールを拡大（元のサイズ × scaleMultiplier）**
                    obj.transform.localScale *= scaleMultiplier;
                }

                currentStage++; // **次のオブジェクトへ進める**
            }
        }
    }
}