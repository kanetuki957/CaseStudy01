using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NRTEST02 : MonoBehaviour
{
    // ���������I�u�W�F�N�g��Player(�^�O��)�Ȃ�
    // �u���������v�ƊO���ɖ��߂��o��

    // �O���ւ̖���
    public bool isHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isHit = true;
        }
    }

    // �����������������s������A��������s����isHit���ēxfalse��
    public void ResetHit()
    {
        isHit = false;
    }



}
