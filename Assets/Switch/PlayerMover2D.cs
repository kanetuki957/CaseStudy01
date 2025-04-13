using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover2D : MonoBehaviour
{
    public float speed = 2f;              // �ړ����x
    public float moveDistance = 5f;       // ������Ɉړ����鋗��

    private Vector3 initialPosition;      // �����ʒu
    private Vector3 startPosition;        // 1�����̊J�n�ʒu
    private int direction = 1;            // ���݂̈ړ������i1 = �E, -1 = ���j

    private bool moving = false;          // ���݈ړ������ǂ���

    void Start()
    {
        initialPosition = transform.position; // �����ʒu��ۑ�
    }

    void Update()
    {
        if (moving)
        {
            // ���E�Ɉړ�
            transform.position += Vector3.right * direction * speed * Time.deltaTime;

            // ��苗���ړ�����������]��
            if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            {
                direction *= -1;
                startPosition = transform.position;
            }
        }
    }

    // �N���b�N�ŌĂ΂��؂�ւ�����
    public void ToggleMove()
    {
        if (!moving)
        {
            // �ړ��J�n
            moving = true;
            startPosition = transform.position;
            direction = 1; // ����͉E��
        }
        else
        {
            // �ړ���~�������ɏ����ʒu�֖߂�
            moving = false;
            transform.position = initialPosition;
        }
    }
}