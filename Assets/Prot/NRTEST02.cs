using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NRTEST02 : MonoBehaviour
{
    // 当たったオブジェクトがPlayer(タグ名)なら
    // 「あたった」と外部に命令を出す

    // 外部への命令
    public bool isHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isHit = true;
        }
    }

    // 当たった処理を実行した後、これを実行してisHitを再度falseに
    public void ResetHit()
    {
        isHit = false;
    }



}
