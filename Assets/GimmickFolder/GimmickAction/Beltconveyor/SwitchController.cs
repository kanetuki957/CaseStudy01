using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スイッチとなるGameObjectにアタッチし、クリック時に対象オブジェクトを動かす処理
public class SwitchController : MonoBehaviour
{
    // 対象のオブジェクト
    public GameObject targetObject;

    // このGameObjectがクリックされたときに呼ばれる処理（マウス操作）
    void OnMouseDown()
    {
        // 対象オブジェクトが設定されているか確認
        if (targetObject != null)
        {
            // MoveTarget スクリプトを探す
            var moveScript = targetObject.GetComponent<MoveBeltconveyor>();
            if (moveScript != null)
            {
                // 対象オブジェクトが設定されているか確認
                moveScript.StartMovingLeft();
            }
        }
    }
}