using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    public float speed = 2f;   // �ړ����x
    public float distance = 3f; // �ړ��͈�
    private float randomOffset; // �I�u�W�F�N�g���Ƃ̃I�t�Z�b�g�l

    void Start()
    {
        randomOffset = Random.Range(-1f, 1f); // �e�I�u�W�F�N�g�ɈقȂ�I�t�Z�b�g��ݒ�
    }

    void Update()
    {
        float posX = Mathf.PingPong((Time.time + randomOffset) * speed, distance * 2) - distance;
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }
}
