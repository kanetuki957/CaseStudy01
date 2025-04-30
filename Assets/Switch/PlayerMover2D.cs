using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover2D : MonoBehaviour
{
    public float speed = 2f;              // �v���C���[�̈ړ����x
    public float moveDistance = 5f;       // 1��̉����ňړ����鋗��
    public int initialDirection = 1;      // ���������i1 = �E, -1 = ���j

    private Vector3 initialPosition;      // �����ʒu�i�߂�Ƃ��̊�j
    private Vector3 startPosition;        // 1�����̊J�n�n�_
    private int direction;                // ���݂̈ړ�����
    private bool moving = false;          // ���݈ړ������ǂ���

    void Start()
    {
        // �����ʒu��ۑ��i�N���b�N�Ŗ߂����߁j
        initialPosition = transform.position;

        // �����̈ړ�������ݒ�
        direction = initialDirection;
    }

    void Update()
    {
        if (moving)
        {
            // �w������Ɉړ��i���t���[���j
            transform.position += Vector3.right * direction * speed * Time.deltaTime;

            // �w�苗���ȏ�ړ������甽�]�i�����^���j
            if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            {
                direction *= -1;                   // �ړ������𔽓]
                startPosition = transform.position;// �V���ȏo���_�ɍX�V
            }
        }
    }

    // �O������Ăяo���Ĉړ��̊J�n/��~��؂�ւ���֐�
    public void ToggleMove()
    {
        if (!moving)
        {
            // �ړ����J�n
            moving = true;

            // ���������ōĊJ����i�N���b�N���ɏ������j
            direction = initialDirection;

            // ���݈ʒu���o���_�Ƃ��ċL�^
            startPosition = transform.position;
        }
        else
        {
            // �ړ����~���A�����ʒu�֑����ɖ߂�
            moving = false;
            transform.position = initialPosition;
        }
    }
}