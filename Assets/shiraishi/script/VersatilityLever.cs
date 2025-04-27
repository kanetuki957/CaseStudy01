using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersatilityLever : MonoBehaviour
{
    [Header("起動したいオブジェクト (IActivatableを継承)")]
    public MonoBehaviour target; // IActivatableを継承したクラスを持ったオブジェクトを入れる

    private Collider2D playerCollider2D;    // プレイヤーの判定

    [Header("起動設定")]
    public bool activateOnTouch = false; // trueなら触れた瞬間に起動

    [Header("起動用キー (activateOnTouch=falseの時だけ有効)")]
    public KeyCode activateKey = KeyCode.F; // 起動用のキー

    private bool hasActivated = false; // 「触れただけ起動」用の一回制御

    void Update()
    {
        if (!activateOnTouch && playerCollider2D != null && Input.GetKeyDown(activateKey))
        {
            // キー押しモードのときは hasActivated無視してすぐ発動
            TryActivate(ignoreHasActivated: true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollider2D = other;

            if (activateOnTouch)
            {
                TryActivate(ignoreHasActivated: false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollider2D = null;
            hasActivated = false; // 触れた起動用だけリセット
        }
    }

    private void TryActivate(bool ignoreHasActivated)
    {
        if (!ignoreHasActivated && hasActivated) return;

        if (target is IActivatable activatable)
        {
            activatable.Activate();
            Debug.Log("レバーが処理を起動しました");

            if (!ignoreHasActivated)
            {
                hasActivated = true; // 「触れただけ」のときだけロック
            }
        }
        else
        {
            Debug.LogWarning("target に IActivatable が実装されていません");
        }
    }
}
