using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floar2 : MonoBehaviour
{
    public float moveDistance = 3f;   // 左右の移動幅（片道分）
    public float moveSpeed = 2f;      // 移動速度（高いほど速い）

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

            // 最初に左に振ってから右へ移動（往復）
            float x = Mathf.PingPong(elapsed * moveSpeed, moveDistance * 2) - moveDistance;
            transform.position = new Vector3(startPos.x + x, startPos.y, startPos.z);
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= HandleGameStateChange;
    }

    void HandleGameStateChange(GameState prev, GameState curr)
    {
        if (curr == GameState.Playing && prev != GameState.Playing)
        {
            playStartTime = Time.time;
        }
    }
}
