using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    [Header("移動の設定")]
    public float moveDistance = 2f;  // 上下に動く距離
    public float moveSpeed = 2f;     // 動くスピード

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Sin波を使って上下に移動
        float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

}
