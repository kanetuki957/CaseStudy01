using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance; // シングルトンインスタンス

    //ＨＰバーの管理クラス（HealthBar）を参照する
    [SerializeField]
    private HealthBar m_healthbar;

    private float m_fHealth;           //現在のＨＰを管理する変数（0.0f～1.0fの範囲）
    private bool isDecreasing = false; //ＨＰ減少処理が実行中かどうか

    void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
        }
    }

    //ゲーム開始時に呼ばれる関数（初期設定）
    void Start()
    {
        //ＨＰを最大（1.0f）に設定
        m_fHealth = 1.0f;
        m_healthbar.SetSize(m_fHealth);
    }

    // **敵と衝突した時にHPをゆっくり減らす関数**
    public void DecreaseHealth(float amount, float duration)
    {
        if (!isDecreasing) // すでに減少処理中ならスキップ
        {
            StartCoroutine(SlowDecreaseHealth(amount, duration));
        }
    }

    // **HPを徐々に減らすコルーチン**
    private IEnumerator SlowDecreaseHealth(float amount, float duration)
    {
        isDecreasing = true;
        float startHealth = m_fHealth;
        float endHealth = Mathf.Max(m_fHealth - amount, 0.0f); // 最小値 0.0 にする
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            m_fHealth = Mathf.Lerp(startHealth, endHealth, elapsedTime / duration);
            m_healthbar.SetSize(m_fHealth);
            yield return null;
        }

        m_fHealth = endHealth; // 最終値を正確に設定
        isDecreasing = false;
    }

    public float GetHealth()
    {
        return m_fHealth;
    }
}
