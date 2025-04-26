using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMover : MonoBehaviour
{
    // プレイヤーの移動速度
    public float speed = 2f;

    // 往復運動の移動距離（画面端で反転して戻る距離）
    public float moveDistance = 5f;

    // すべてのアイテム取得後、最初に向かう右端のX座標
    public float moveToRightEdgeX = 10f;

    // アイテム取得数を表示する TextMeshPro UI テキスト
    public TMP_Text countText;

    // アイテムとして扱うオブジェクト名に含まれる文字列
    public string itemKeyword = "Item";

    // 取得対象のアイテムオブジェクト一覧（名前でフィルタ済み）
    private List<GameObject> itemList = new List<GameObject>();

    // 現在までに取得したアイテム数
    private int collectedCount = 0;

    // 全てのアイテムを取得し終わったかどうかのフラグ
    private bool allCollected = false;

    // 右端に到達し、往復運動を開始しているかどうかのフラグ
    private bool startPingPong = false;

    // 往復移動の起点となる位置
    private Vector3 startPosition;

    // 現在の移動方向（1 = 右、-1 = 左）
    private int direction = 1;

    // 初期化処理（アイテム検出とUI初期表示）
    void Start()
    {
        // シーン内に存在するすべてのオブジェクトを走査
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // オブジェクト名にitemが含まれているものをアイテムとして登録
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains(itemKeyword))
            {
                itemList.Add(obj);
            }
        }

        // UIテキストを初期表示（0）
        UpdateText();

        // 最初の移動開始地点を記録
        startPosition = transform.position;
    }

    // 毎フレーム呼ばれる処理
    void Update()
    {
        if (!allCollected)
        {
            // アイテム未取得中：常に右に移動
            transform.position += Vector3.right * speed * Time.deltaTime;

            // アイテムを全て取得したらフラグを更新
            if (itemList.Count == 0)
            {
                allCollected = true;
            }
        }
        else if (!startPingPong)
        {
            // 全取得後：一度だけ右端（moveToRightEdgeX）まで移動
            transform.position += Vector3.right * speed * Time.deltaTime;

            if (transform.position.x >= moveToRightEdgeX)
            {
                // 右端に到達 → 往復運動モードに移行
                startPosition = transform.position;
                direction = -1; // 左へ移動開始
                startPingPong = true;
            }
        }
        else
        {
            // 往復移動モード（moveDistanceの範囲で左右に移動）
            transform.position += Vector3.right * direction * speed * Time.deltaTime;

            // 一定距離進んだら移動方向を反転
            if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            {
                direction *= -1;                    // 反転（1 ? -1）
                startPosition = transform.position; // 現在位置を新たな起点に
            }
        }
    }

    // アイテムとの衝突を検出する関数
    void OnTriggerEnter2D(Collider2D other)
    {
        // 名前にitemを含むものだけをアイテムと判定
        if (other.gameObject.name.Contains(itemKeyword))
        {
            collectedCount++;                 // カウント加算
            UpdateText();                     // UI更新
            Destroy(other.gameObject);        // アイテムを削除
            itemList.Remove(other.gameObject);// リストからも削除
        }
    }

    // UIテキストに現在のカウントを反映する関数
    void UpdateText()
    {
        countText.text = collectedCount.ToString();
    }
}