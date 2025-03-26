using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform m_tfBar; //ＨＰバーのTransform
    private Animator m_animator; //アニメーターの参照

    //初期化（オブジェクトの取得）
    private void Awake()
    {
        //"Bar"という名前の子オブジェクトを探して格納
        m_tfBar = transform.Find("Bar");

        //アニメーターの取得
        m_animator = GetComponent<Animator>();

        //初期状態ではhealth_rateを1.0に設定
        if (m_animator != null)
        {
            m_animator.SetFloat("health_rate", 1.0f);
        }
    }

    //ＨＰバーのサイズを変更する関数
    public void SetSize(float _fNormalize)
    {
        // HPバーのサイズ変更
        m_tfBar.localScale = new Vector3(_fNormalize, 1.0f);

        // アニメーターに現在のＨＰ割合を渡し、アニメーションを制御
        if (m_animator != null)
        {
            m_animator.SetFloat("health_rate", _fNormalize);
        }
    }
}
