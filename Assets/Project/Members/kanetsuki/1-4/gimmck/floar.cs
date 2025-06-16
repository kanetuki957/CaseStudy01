using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floar : MonoBehaviour
{
    public float moveDistance = 3f;   // 左右の最大移動距離（振幅）
    public float moveSpeed = 2f;      // 動く速さ（周波数）

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float x = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(startPos.x + x, startPos.y, startPos.z);
    }
}
