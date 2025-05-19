using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldMover : MonoBehaviour
{
    private Vector2 pos;
    public int num = 1;

    public float leftLimit = -3f;  // 左端
    public float rightLimit = 3f;  // 右端

    void Update()
    {
        pos = transform.position;

        // 移動処理
        transform.Translate(transform.right * Time.deltaTime * 3 * num);

        // 範囲チェック
        if (pos.x > rightLimit)
        {
            num = -1; // 左へ
        }
        if (pos.x < leftLimit)
        {
            num = 1;  // 右へ
        }
    }

}
