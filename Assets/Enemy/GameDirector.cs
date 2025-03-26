using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance; // �V���O���g���C���X�^���X

    //�g�o�o�[�̊Ǘ��N���X�iHealthBar�j���Q�Ƃ���
    [SerializeField]
    private HealthBar m_healthbar;

    private float m_fHealth;           //���݂̂g�o���Ǘ�����ϐ��i0.0f�`1.0f�͈̔́j
    private bool isDecreasing = false; //�g�o�������������s�����ǂ���

    void Awake()
    {
        // �V���O���g���̐ݒ�
        if (Instance == null)
        {
            Instance = this;
        }
    }

    //�Q�[���J�n���ɌĂ΂��֐��i�����ݒ�j
    void Start()
    {
        //�g�o���ő�i1.0f�j�ɐݒ�
        m_fHealth = 1.0f;
        m_healthbar.SetSize(m_fHealth);
    }

    // **�G�ƏՓ˂�������HP��������茸�炷�֐�**
    public void DecreaseHealth(float amount, float duration)
    {
        if (!isDecreasing) // ���łɌ����������Ȃ�X�L�b�v
        {
            StartCoroutine(SlowDecreaseHealth(amount, duration));
        }
    }

    // **HP�����X�Ɍ��炷�R���[�`��**
    private IEnumerator SlowDecreaseHealth(float amount, float duration)
    {
        isDecreasing = true;
        float startHealth = m_fHealth;
        float endHealth = Mathf.Max(m_fHealth - amount, 0.0f); // �ŏ��l 0.0 �ɂ���
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            m_fHealth = Mathf.Lerp(startHealth, endHealth, elapsedTime / duration);
            m_healthbar.SetSize(m_fHealth);
            yield return null;
        }

        m_fHealth = endHealth; // �ŏI�l�𐳊m�ɐݒ�
        isDecreasing = false;
    }

    public float GetHealth()
    {
        return m_fHealth;
    }
}
