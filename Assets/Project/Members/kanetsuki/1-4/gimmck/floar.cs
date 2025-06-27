using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floar : MonoBehaviour
{
    public float moveDistance = 3f;   // 左右の最大移動距離（振幅）
    public float moveSpeed = 2f;      // 動く速さ（周波数）

    private Vector3 startPos;
    private float playStartTime = 0f;

    void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChange;
        startPos = transform.position;
        playStartTime = Time.time;
    }

    void Update()
    {
        if (GameManager.Instance.currentState == GameState.Playing)
        {
            float elapsed = Time.time - playStartTime;
            float x = Mathf.Sin(elapsed * moveSpeed) * moveDistance;
            transform.position = new Vector3(startPos.x + x, startPos.y, startPos.z);
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= HandleGameStateChange;
    }

    // ゲームステートの変更を検知して処理を実行
    void HandleGameStateChange(GameState prev, GameState curr)
    {
        if (curr == GameState.Playing && prev != GameState.Playing)
        {
            playStartTime = Time.time;
        }
    }
}
