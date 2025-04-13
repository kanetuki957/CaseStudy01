using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover2D : MonoBehaviour
{
    public float speed = 2f;              // �v���C���[�̈ړ����x
    public float moveDistance = 5f;       // ������Ɉړ����鋗��

    private Vector3 startPosition;        // �ړ��J�n�n�_
    private bool moving = false;          // �ړ����t���O
    private int direction = 1;            // ���݂̈ړ������i1 = �E, -1 = ���j

    void Update()
    {
        if (moving)
        {
            // �E�܂��͍��Ɉړ�
            transform.position += Vector3.right * direction * speed * Time.deltaTime;

            // �w�肵�������܂ňړ������甽�]
            if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            {
                direction *= -1;                   // �����𔽓]
                startPosition = transform.position;// �ړ��J�n�n�_���X�V
            }
        }
    }

    // �ړ��J�n�֐��i�O������Ăяo���j
    public void StartMoving()
    {
        if (!moving)
        {
            moving = true;
            startPosition = transform.position;
        }
    }
}