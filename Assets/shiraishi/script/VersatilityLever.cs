using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VersatilityLever : MonoBehaviour
{
    [Header("起動したいオブジェクト(IActivatableを継承)")]
    public MonoBehaviour target; // IActivatableを継承したクラスを持ったオブジェクトを入れる

    private Collider2D playerCollider2D;    // プレイヤーの判定

    [Header("起動用キー")]
    public KeyCode activateKey = KeyCode.F; // 起動用のキー


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // キーが入力される　かつ　プレイヤーがレバーに重なっている
        if (Input.GetKeyDown(activateKey) && playerCollider2D != null)
        {
            // ターゲットのオブジェクトがIActivatableを継承したクラスか判定
            if (target is IActivatable activatable)
            {
                // ターゲットが持っているActivateを実行
                activatable.Activate();

                Debug.Log("レバーが処理を起動しました");
            }
            else
            {
                Debug.LogWarning("target に IActivatable が実装されていません");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollider2D = other;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollider2D = null;
        }
    }
}
