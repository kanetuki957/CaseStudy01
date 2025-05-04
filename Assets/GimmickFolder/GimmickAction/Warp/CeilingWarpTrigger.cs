using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingWarpTrigger : MonoBehaviour
{
    public GameObject warpTarget; // ���[�v��i�ړI�n�ƂȂ�I�u�W�F�N�g�j

    void OnTriggerEnter2D(Collider2D other)
    {
        // �L�����N�^�[��GravityCharacter�X�N���v�g�����邩�m�F
        var character = other.GetComponent<GravityCharacter>();
        if (character != null)
        {
            // �L�����N�^�[���w��ʒu�փ��[�v
            character.WarpTo(warpTarget);
        }
    }
}
