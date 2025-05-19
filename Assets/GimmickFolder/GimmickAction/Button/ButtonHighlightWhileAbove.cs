using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHighlightWhileAbove : MonoBehaviour
{
    // キャラクターオブジェクト
    public GameObject characterObject;

    // 通常の色（真上にいないとき）
    public Color normalColor = Color.white;

    // 真上にいるときの紫色
    public Color highlightColor = Color.magenta;

    // 色を切り替えるためのSpriteRenderer
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 真上にいるときだけ紫に、それ以外は元の色に
        if (IsCharacterAbove())
        {
            sr.color = highlightColor;
        }
        else
        {
            sr.color = normalColor;
        }
    }

    // キャラが真上にいるかを判定する
    bool IsCharacterAbove()
    {
        Vector3 charPos = characterObject.transform.position;
        Vector3 btnPos = transform.position;

        return Mathf.Abs(charPos.x - btnPos.x) < 0.5f && charPos.y > btnPos.y;
    }
}
