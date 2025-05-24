using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloorOnClick : MonoBehaviour
{
    // 動かす対象の床オブジェクト
    public GameObject floorObject;

    // キャラクター（移動中かどうかを判定する）
    public GameObject characterObject;

    // 床がどれだけ上に動くか
    public float moveDistance = 2f;

    // 床が動く時間
    public float moveDuration = 1f;

    // 1回だけ動くようにするフラグ
    private bool hasMoved = false;

    // オブジェクトがクリックされたときの処理
    void OnMouseDown()
    {
        // すでに動かしたら無視
        if (hasMoved) return;

        // キャラクターが動いている場合だけ実行
        var rb = characterObject.GetComponent<Rigidbody2D>();
        if (rb != null && Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            hasMoved = true;
            StartCoroutine(MoveFloorSmoothly());
        }
    }

    // 床をなめらかに上に動かすコルーチン処理
    System.Collections.IEnumerator MoveFloorSmoothly()
    {
        Vector3 startPos = floorObject.transform.position;
        Vector3 endPos = startPos + new Vector3(0f, moveDistance, 0f);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            floorObject.transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 最後に正確な位置に合わせて終了
        floorObject.transform.position = endPos;
    }
}
