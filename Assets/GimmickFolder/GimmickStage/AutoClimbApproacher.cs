using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �y�L�����N�^�[�������Ńn�V�S�֌������Ĉړ����āA�o��~����P�񂾂��s���X�N���v�g�z
public class AutoClimbApproacher : MonoBehaviour
{
    public GameObject ladderTarget;    // �o��Ɍ������n�V�S�̃^�[�Q�b�g
    public float moveSpeed = 2f;       // ���ֈړ�����Ƃ��E�o��Ƃ��̃X�s�[�h
    public float snapDistance = 1f;    // �n�V�S�ɋz���t�������i������߂Â�����z������j
    public float topY = 5f;            // �n�V�S��o�肫�鍂���i�ڕWY���W�j
    public float bottomY = 0f;         // �n�V�S�����肫�鍂���i�ڕWY���W�j
    public float climbOffsetX = 0f;    // �n�V�S�Ƃ�X���W�̕␳�i���炵�����ꍇ�j

    private Rigidbody2D rb;            // Rigidbody2D�R���|�[�l���g�����Ă����ϐ�

    // �L�����N�^�[�̌��݂̍s���X�e�[�g�i�������Ă��邩�j
    private enum State { MoveToLadder, SnapToLadder, ClimbUp, ClimbDown }
    private State currentState = State.MoveToLadder; // �ŏ��̓n�V�S�Ɍ������Ƃ��납��X�^�[�g

    private bool finishedClimbing = false; // �n�V�S��o��~�肵���ォ�ǂ������L�^����t���O

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D���擾���Ă���
    }

    void Update()
    {
        // �����o��~�肪�I����Ă���A�����������Ȃ�
        if (finishedClimbing) return;

        // ���݂̃X�e�[�g�i��ԁj�ɂ���Ă�邱�Ƃ�؂�ւ���
        switch (currentState)
        {
            case State.MoveToLadder:
                MoveLeft();                   // ���Ɉړ�����
                CheckSnapDistance();          // �n�V�S�ɋ߂Â������`�F�b�N����
                break;

            case State.SnapToLadder:
                SnapToLadder();               // �n�V�S�ɂ҂�����z������
                currentState = State.ClimbUp; // ���͓o��n�߂�X�e�[�g�ɐ؂�ւ���
                break;

            case State.ClimbUp:
                Climb(1);                     // ��Ɍ������ēo��
                break;

            case State.ClimbDown:
                Climb(-1);                    // ���Ɍ������č~���
                break;
        }
    }

    // �������Ɉړ�����֐�
    void MoveLeft()
    {
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); // ����moveSpeed�̑����Ői��
    }

    // �n�V�S�ɋ߂Â������ǂ������肷��֐�
    void CheckSnapDistance()
    {
        float dist = Mathf.Abs(
            transform.position.x - ladderTarget.transform.position.x); // �n�V�S�Ƃ̉��������v�Z

        if (dist <= snapDistance) // ���e�͈͓��ɓ�������
        {
            currentState = State.SnapToLadder; // �X�e�[�g���z�����[�h�ɕύX
        }
    }

    // �n�V�S�ɂ҂�����X���W�����킹��֐�
    void SnapToLadder()
    {
        // �ړ����~�߂āAX�ʒu���҂����荇�킹��
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(
            ladderTarget.transform.position.x + climbOffsetX,
            transform.position.y,
            transform.position.z
        );
    }

    // �n�V�S��o������~�肽�肷��֐�
    void Climb(int direction)
    {
        rb.velocity = new Vector2(0, moveSpeed * direction); // �㉺�����ɐi�ށidirection=1�Ȃ��A-1�Ȃ牺�j

        // �o�肫������
        if (direction == 1 && transform.position.y >= topY)
        {
            rb.velocity = Vector2.zero;     // �������~�߂�
            transform.position = new Vector3(
                transform.position.x,
                topY,
                transform.position.z);      // ���m�Ȉʒu�ɍ��킹��

            currentState = State.ClimbDown; // ���͉���X�e�[�g�ɐ؂�ւ�
        }

        // ���肫������
        else if (direction == -1 && transform.position.y <= bottomY)
        {
            rb.velocity = Vector2.zero; // �������~�߂�
            transform.position = new Vector3(
                transform.position.x,
                bottomY,
                transform.position.z);  // ���m�Ȉʒu�ɍ��킹��

            finishedClimbing = true;    // �o��~�肪���������ƃ}�[�N����

            // MoveToTargetAfterClimb�X�N���v�g��T���āA���̃^�[�Q�b�g�Ɍ����Ĉړ����J�n������
            FindObjectOfType<MoveToTargetAfterClimb>().StartMoving();
        }
    }
}