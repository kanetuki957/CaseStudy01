using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingWarpTrigger : MonoBehaviour
{
    public GameObject warpTarget; // ワープ先（目的地となるオブジェクト）

    void OnTriggerEnter2D(Collider2D other)
    {
        // キャラクターにGravityCharacterスクリプトがあるか確認
        var character = other.GetComponent<GravityCharacter>();
        if (character != null)
        {
            // キャラクターを指定位置へワープ
            character.WarpTo(warpTarget);
        }
    }
}
