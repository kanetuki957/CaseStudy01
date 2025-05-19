using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キャラクターが触れたら磁力で引き寄せるトリガー
public class GravityTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var character = other.GetComponent<GravityCharacter>();
        if (character != null)
        {
            // キャラクターに上昇開始命令を出す
            character.TriggerFloatUp();
        }
    }
}
