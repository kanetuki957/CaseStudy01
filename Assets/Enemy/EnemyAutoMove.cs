using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAutoMove : MonoBehaviour
{
    public float speed = 3f;      // �ړ����x
    public float leftLimit = -5f; // ���[�̈ʒu
    public float rightLimit = 5f; // �E�[�̈ʒu
    private int direction = 1;    // �ړ������i1: �E, -1: ���j

    void Update()
    {
        // �G���ړ�������
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // ���[�܂��͉E�[�ɒB������ړ�������ς���
        if (transform.position.x <= leftLimit)
        {
            direction = 1; // �E�Ɉړ�
        }
        else if (transform.position.x >= rightLimit)
        {
            direction = -1; // ���Ɉړ�
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameDirector.Instance != null)
            {
                //�g�o���u�R�b�Ԃ����āv0.3���炷
                GameDirector.Instance.DecreaseHealth(0.3f, 3f);
            }

            direction = 1;
        }
    }

}
