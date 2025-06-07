using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator2 : MonoBehaviour
{
    [System.Serializable]
    public class SwitchData
    {
        public GameObject switchObject;
        public GameObject targetObject;
        public float moveDistance = 2f;
        public float moveSpeed = 2f;

        [HideInInspector] public Vector3 topPos;
        [HideInInspector] public Vector3 bottomPos;
        [HideInInspector] public bool activated = false;
        [HideInInspector] public bool goingUp = true;
        [HideInInspector] public bool hasStartedMoving = false; // 動き始めたかどうか
    }

    public List<SwitchData> switches = new List<SwitchData>();

    public AudioClip switchSE;   // スイッチ音
    public AudioClip moveSE;     // 動作開始音
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        foreach (var s in switches)
        {
            if (s.targetObject != null)
            {
                s.bottomPos = s.targetObject.transform.position;
                s.topPos = s.bottomPos + Vector3.up * s.moveDistance;
                s.goingUp = true;
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

                // スイッチ反転
                Vector3 scale = s.switchObject.transform.localScale;
                scale.x *= -1;
                s.switchObject.transform.localScale = scale;

                // SE（スイッチ用）
                if (switchSE != null)
                    audioSource.PlayOneShot(switchSE);
            }
        }
    }

    void MoveTarget(SwitchData s)
    {
        Vector3 destination = s.goingUp ? s.topPos : s.bottomPos;

        // 初回移動開始時にSE（移動SE）
        if (!s.hasStartedMoving)
        {
            s.hasStartedMoving = true;
            if (moveSE != null)
                audioSource.PlayOneShot(moveSE);
        }

        s.targetObject.transform.position = Vector3.MoveTowards(
            s.targetObject.transform.position,
            destination,
            s.moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(s.targetObject.transform.position, destination) < 0.01f)
        {
            s.goingUp = !s.goingUp;
            s.hasStartedMoving = false; // 次の方向転換時にも鳴らせるようにリセット
        }
    }

}
