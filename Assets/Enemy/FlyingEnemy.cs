using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float moveSpeed = 1f;       // �������ւ̈ړ����x
    public float floatSpeed = 2f;      // �㉺�̗h��̑���
    public float floatHeight = 2f;     // �㉺�̐U�ꕝ

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // ���ɏ������ړ�
        float moveX = transform.position.x - moveSpeed * Time.deltaTime;

        // �㉺�ɂ����
        float moveY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // �V�����ʒu�Ɉړ�
        transform.position = new Vector3(moveX, moveY, transform.position.z);
    }
}
