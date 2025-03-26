using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform m_tfBar; //�g�o�o�[��Transform
    private Animator m_animator; //�A�j���[�^�[�̎Q��

    //�������i�I�u�W�F�N�g�̎擾�j
    private void Awake()
    {
        //"Bar"�Ƃ������O�̎q�I�u�W�F�N�g��T���Ċi�[
        m_tfBar = transform.Find("Bar");

        //�A�j���[�^�[�̎擾
        m_animator = GetComponent<Animator>();

        //������Ԃł�health_rate��1.0�ɐݒ�
        if (m_animator != null)
        {
            m_animator.SetFloat("health_rate", 1.0f);
        }
    }

    //�g�o�o�[�̃T�C�Y��ύX����֐�
    public void SetSize(float _fNormalize)
    {
        // HP�o�[�̃T�C�Y�ύX
        m_tfBar.localScale = new Vector3(_fNormalize, 1.0f);

        // �A�j���[�^�[�Ɍ��݂̂g�o������n���A�A�j���[�V�����𐧌�
        if (m_animator != null)
        {
            m_animator.SetFloat("health_rate", _fNormalize);
        }
    }
}
