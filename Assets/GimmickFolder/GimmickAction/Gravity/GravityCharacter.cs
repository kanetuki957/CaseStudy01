using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityCharacter : MonoBehaviour
{
    public float moveSpeed = 3f;         // �E�ړ����x
    public float floatUpSpeed = 5f;      // �㏸���x
    public float upperLimitY = 4.5f;     // �V��ʒuY

    public float warpOffsetX = 1.0f; // ���[�v�ʒu��X�����ɂ��炷�l

    private Rigidbody2D rb;

    // �L�����N�^�[�̏�ԁi�n�� �� ���� �� ����t�� �� �V��ړ� �� ���[�v��~�j
    private enum State
    {
        Walking,
        FloatingUp,
        StuckToCeiling,
        CeilingMoving,
        Warping
    }

    private State currentState = State.Walking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // �����͏d�͂Ȃ��i���S�蓮����j
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Walking:
                // �n�ʂ��E�ɕ���
                rb.velocity = new Vector2(moveSpeed, 0f);
                break;

            case State.FloatingUp:
                // �^��ɂ������ړ��i�z���񂹒��j
                rb.velocity = new Vector2(0f, floatUpSpeed);
                if (transform.position.y >= upperLimitY)
                {
                    rb.velocity = Vector2.zero;
                    rb.gravityScale = 0f; // �d��OFF
                    FlipVertically();
                    currentState = State.StuckToCeiling;
                }
                break;

            case State.StuckToCeiling:
                // �����̒x����ɓV��ړ����J�n�i�܂��͂����؂�ւ��ł�OK�j
                currentState = State.CeilingMoving;
                break;

            case State.CeilingMoving:
                // �V���ŉE�ֈړ�
                rb.velocity = new Vector2(moveSpeed, 0f);
                break;

            case State.Warping:
                // ���S�Ɉړ���~�i���[�v����j
                rb.velocity = Vector2.zero;
                break;
        }
    }

    // �㏸���J�n����֐�
    public void TriggerFloatUp()
    {
        if (currentState == State.Walking)
        {
            currentState = State.FloatingUp;
        }
    }

    private void FlipVertically()
    {
        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;
    }

    // ���I�u�W�F�N�g����Ă΂�ďu�Ԉړ�����֐�
    public void WarpTo(GameObject target)
    {
        if (target == null) return;

        // ���[�v��̍��W���擾���āAX�����ɃI�t�Z�b�g��������
        Vector3 targetPos = target.transform.position;
        targetPos.x += warpOffsetX;

        // �L���������炵���ʒu�Ƀ��[�v
        transform.position = targetPos;

        // ��Ԑ؂�ւ��i���[�v���~�j
        currentState = State.Warping;
        rb.velocity = Vector2.zero;
    }
}