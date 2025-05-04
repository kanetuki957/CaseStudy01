using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHighlightOneTime : MonoBehaviour
{
    // キャラクターオブジェクト
    public GameObject characterObject;

    // ボタンが紫に変わる色設定
    public Color highlightColor = Color.magenta;

    // 一度反応したかどうか
    private bool activated = false;

    // ボタンの色を変えるためのSpriteRenderer
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 一度でも反応したら何もしない
        if (activated) return;

        // キャラクターが真上にいるか確認
        if (IsCharacterAbove())
        {
            sr.color = highlightColor;
            activated = true;
        }
    }

    // キャラがボタンの真上にいるかチェックする関数
    bool IsCharacterAbove()
    {
        Vector3 charPos = characterObject.transform.position;
        Vector3 btnPos = transform.position;

        return Mathf.Abs(charPos.x - btnPos.x) < 0.5f && charPos.y > btnPos.y;
    }
}
