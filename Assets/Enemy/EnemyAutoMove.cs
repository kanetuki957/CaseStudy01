using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAutoMove : MonoBehaviour
{
    public float speed = 3f;      // **�ړ����x�i�P��: ���j�b�g/�b�j**
    public float leftLimit = -5f; // **���[�̈ʒu�i���̍��W�ɒB����ƉE�ֈړ��j**
    public float rightLimit = 5f; // **�E�[�̈ʒu�i���̍��W�ɒB����ƍ��ֈړ��j**
    private int direction = 1;    // **�ړ������i1: �E, -1: ���j**

    // �v���C���[�̃I�u�W�F�N�g��
    public string playerObjectName;

    void Update()
    {
        // **�G�����݂̈ړ������Ɉړ�������**
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // **���[�܂��͉E�[�ɒB������ړ������𔽓]**
        if (transform.position.x <= leftLimit)
        {
            direction = 1; // **�E�ֈړ�**
        }
        else if (transform.position.x >= rightLimit)
        {
            direction = -1; // **���ֈړ�**
        }
    }

    // **�v���C���[�ƏՓ˂����ۂ̏���**
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Փ˂����I�u�W�F�N�g�Ȃ珈�������s
        if (collision.gameObject.name == playerObjectName)
        {
            if (GameDirector.Instance != null)
            {
                // **�e�N�X�`����ύX**
                GameDirector.Instance.ChangeTexture();
            }

            direction = 1; // **�G�̈ړ��������E�ɌŒ�**
        }
    }
}