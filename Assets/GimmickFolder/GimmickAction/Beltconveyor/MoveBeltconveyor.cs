using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �w���GameObject�����Ɉړ������鏈�����Ǘ�����X�N���v�g
public class MoveBeltconveyor : MonoBehaviour
{
    // �������ւ̈ړ����x
    public float moveSpeed = 2f;

    // ����X���W��荶�ɗ�����ړ����~����
    public float stopXPosition = -10f;

    // �ړ������ǂ����̃t���O
    private bool isMoving = false;

    void Update()
    {
        // �ړ��t���O�������Ă���Ԃ͍��Ɉړ���������
        if (isMoving)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

            // �w����W��荶�ɏo����~�߂�
            if (transform.position.x <= stopXPosition)
            {
                isMoving = false;
            }
        }
    }

    // �O�����炱�̊֐����ĂԂ��ƂŁA�������ɂ��Ĉړ����J�n����
    public void StartMovingLeft()
    {
        // �E�����ł��K���������ɂ���i�X�P�[�����]�j
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;

        // �ړ��J�n
        isMoving = true;
    }
}