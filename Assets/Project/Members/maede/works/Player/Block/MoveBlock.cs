using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    public float speed = 2f;   // 移動速度
    public float distance = 3f; // 移動範囲
    private float randomOffset; // オブジェクトごとのオフセット値

    void Start()
    {
        randomOffset = Random.Range(-1f, 1f); // 各オブジェクトに異なるオフセットを設定
    }

    void Update()
    {
        float posX = Mathf.PingPong((Time.time + randomOffset) * speed, distance * 2) - distance;
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }
}
