using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;      // **�ړ����x�i�P��: ���j�b�g/�b�j**
    public float leftLimit = -5f; // **���[�̈ʒu�i���̍��W�ɒB����ƉE�ֈړ��j**
    public float rightLimit = 5f; // **�E�[�̈ʒu�i���̍��W�ɒB����ƍ��ֈړ��j**
    private int direction = 1;    // **�ړ������i1: �E, -1: ���j**

    public GameObject[] playerTextureObjects; // �v���C���[�̃e�N�X�`���ύX�I�u�W�F�N�g
    public Sprite[] playerTextureStages; // �v���C���[�̃e�N�X�`���ꗗ
    public float scaleMultiplier = 1.2f; // �X�P�[���{��
    private int playerCurrentStage = 0; // ���݂̃e�N�X�`���i�K

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

            if (PlayerHealth.Instance != null)
            {
                // **�e�N�X�`����ύX**
                PlayerHealth.Instance.ChangeTexture();
                ChangePlayerTexture(); // �v���C���[�̃e�N�X�`���ύX
            }

            direction = 1; // **�G�̈ړ��������E�ɌŒ�**
        }
    }

    // **�v���C���[�̃e�N�X�`����ύX**
    private void ChangePlayerTexture()
    {
        if (playerCurrentStage < playerTextureObjects.Length)
        {
            GameObject obj = playerTextureObjects[playerCurrentStage];

            if (obj != null)
            {
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = playerTextureStages[playerCurrentStage];
                    obj.transform.localScale *= scaleMultiplier;
                }
                playerCurrentStage++;
            }
        }
    }
}