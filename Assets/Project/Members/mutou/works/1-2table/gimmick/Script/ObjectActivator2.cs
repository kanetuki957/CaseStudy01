using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スイッチに触れると対象オブジェクトが上下に動き、効果音が鳴るクラス
public class ObjectActivator2 : MonoBehaviour
{
    // スイッチごとの設定データ
    [System.Serializable]
    public class SwitchData
    {
        public GameObject switchObject; // スイッチ本体（触れる対象）
        public GameObject targetObject; // 上下に動かす対象
        public float moveDistance = 2f; // 動く距離
        public float moveSpeed = 2f;    // 動くスピード

        [HideInInspector] public Vector3 topPos;                // 移動の上端位置
        [HideInInspector] public Vector3 bottomPos;             // 移動の下端位置（初期位置）
        [HideInInspector] public bool activated = false;        // スイッチが起動されたか
        [HideInInspector] public bool goingUp = true;           // 現在の移動方向（true: 上）
        [HideInInspector] public bool hasStartedMoving = false; // 効果音再生制御用フラグ
    }

    public List<SwitchData> switches = new List<SwitchData>(); // スイッチ一覧

    public AudioClip switchSE;   // スイッチを押したときの効果音
    public AudioClip moveSE;     // 対象が動き出すときの効果音
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource を自動追加
        audioSource = gameObject.AddComponent<AudioSource>();

        // 各スイッチの上下移動範囲を初期化
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
        // 起動済みスイッチに対して移動処理を実行
        foreach (var s in switches)
        {
            if (s.activated)
            {
                MoveTarget(s);
            }
        }
    }

    // スイッチにプレイヤーが触れたときの処理
    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var s in switches)
        {
            if (other.gameObject == s.switchObject && !s.activated)
            {
                s.activated = true;

                // スイッチの向きを左右反転
                Vector3 scale = s.switchObject.transform.localScale;
                scale.x *= -1;
                s.switchObject.transform.localScale = scale;

                // スイッチ効果音を再生
                if (switchSE != null)
                    audioSource.PlayOneShot(switchSE);
            }
        }
    }

    // 対象オブジェクトを上下に動かす処理
    void MoveTarget(SwitchData s)
    {
        Vector3 destination = s.goingUp ? s.topPos : s.bottomPos;

        // 初回移動時に効果音を再生
        if (!s.hasStartedMoving)
        {
            s.hasStartedMoving = true;
            if (moveSE != null)
                audioSource.PlayOneShot(moveSE);
        }

        // 対象を目的地まで移動
        s.targetObject.transform.position = Vector3.MoveTowards(
            s.targetObject.transform.position,
            destination,
            s.moveSpeed * Time.deltaTime
        );

        // 到達したら方向を反転し、効果音再生フラグをリセット
        if (Vector3.Distance(s.targetObject.transform.position, destination) < 0.01f)
        {
            s.goingUp = !s.goingUp;
            s.hasStartedMoving = false;
        }
    }

}