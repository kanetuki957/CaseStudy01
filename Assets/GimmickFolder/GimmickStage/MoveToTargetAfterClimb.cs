using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �y�n�V�S��o�������ƂɁA�^�[�Q�b�g�I�u�W�F�N�g�ֈړ����A�G�ꂽ�珉���ʒu�ւ������߂�X�N���v�g�z
public class MoveToTargetAfterClimb : MonoBehaviour
{
    public GameObject targetObject;     // �������Ă����ڕW�I�u�W�F�N�g
    public float moveSpeed = 2f;        // �^�[�Q�b�g�܂ł̈ړ��X�s�[�h
    public float returnSpeed = 1f;      // �����ʒu�ɖ߂�Ƃ��̃X�s�[�h

    private bool canMove = false;       // �ړ����J�n���Ă������ǂ���
    private bool returnToStart = false; // �����ʒu�ɖ߂郂�[�h���ǂ���
    private Vector3 startPosition;      // �ŏ��ɂ����ʒu�i�����ʒu�j

    void Start()
    {
        startPosition = transform.position; // �ŏ��̍��W��ۑ����Ă���
    }

    void Update()
    {
        if (canMove && !returnToStart)
        {
            // �^�[�Q�b�g�Ɍ������Ĉړ�����
            MoveTowardsTarget(targetObject.transform.position);
        }
        else if (returnToStart)
        {
            // �����ʒu�ɖ߂�
            MoveTowardsTarget(startPosition);
        }
    }

    // �O������Ăяo���āu�ړ����J�n����v���߂̊֐�
    public void StartMoving()
    {
        canMove = true; // �ړ����J�nOK�ɂ���
    }

    // �w�肵���^�[�Q�b�g�֌������Ĉړ����鏈��
    void MoveTowardsTarget(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position).normalized;  // �^�[�Q�b�g�ւ̕������v�Z
        float speed = returnToStart ? returnSpeed : moveSpeed;      // �߂�Ƃ���returnSpeed�A����ȊO��moveSpeed���g��

        transform.position += dir * speed * Time.deltaTime;         // �^�[�Q�b�g�����ֈړ�����

        // �^�[�Q�b�g�ɋ߂Â������~����
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            if (!returnToStart)
            {
                // �^�[�Q�b�g�ɓ��������Ƃ� �� �ړ��X�g�b�v
                canMove = false;
            }
            else
            {
                // �����ʒu�ɖ߂����Ƃ� �� �ړ��X�g�b�v
                returnToStart = false;
            }
        }
    }

    // �^�[�Q�b�g�ɐG�ꂽ�Ƃ��̏����iOnTriggerEnter2D�j
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == targetObject)
        {
            // �����^�[�Q�b�g�I�u�W�F�N�g�ɐG�ꂽ��A�����ʒu�ɖ߂郂�[�h�ɐ؂�ւ���
            returnToStart = true;
            canMove = false; // ��������ړ���~�i�߂鏈����Update���œ����j
        }
    }
}