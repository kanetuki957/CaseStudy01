using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator2 : MonoBehaviour
{
    [System.Serializable]
    public class SwitchData
    {
        public GameObject switchObject;     // スイッチ本体
        public GameObject targetObject;     // 動かすオブジェクト
        public float moveDistance = 2f;
        public float moveSpeed = 2f;

        [HideInInspector] public Vector3 topPos;
        [HideInInspector] public Vector3 bottomPos;
        [HideInInspector] public bool activated = false;
        [HideInInspector] public bool goingUp = true; // 下からスタートなので最初は上へ
    }

    public List<SwitchData> switches = new List<SwitchData>();

    void Start()
    {
        foreach (var s in switches)
        {
            if (s.targetObject != null)
            {
                // 現在位置が bottom として基準にする
                s.bottomPos = s.targetObject.transform.position;
                s.topPos = s.bottomPos + Vector3.up * s.moveDistance;
                s.goingUp = true; // 最初は上へ向かう
            }
        }
    }

    void Update()
    {
        foreach (var s in switches)
        {
            if (s.activated)
            {
                MoveTarget(s);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var s in switches)
        {
            if (other.gameObject == s.switchObject && !s.activated)
            {
                s.activated = true;

                // スイッチ反転：左右反転
                Vector3 scale = s.switchObject.transform.localScale;
                scale.x *= -1;
                s.switchObject.transform.localScale = scale;
            }
        }
    }

    void MoveTarget(SwitchData s)
    {
        Vector3 destination = s.goingUp ? s.topPos : s.bottomPos;
        s.targetObject.transform.position = Vector3.MoveTowards(
            s.targetObject.transform.position,
            destination,
            s.moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(s.targetObject.transform.position, destination) < 0.01f)
        {
            s.goingUp = !s.goingUp;
        }
    }
}
