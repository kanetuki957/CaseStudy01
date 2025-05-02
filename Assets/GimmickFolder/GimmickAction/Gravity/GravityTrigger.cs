using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �L�����N�^�[���G�ꂽ�玥�͂ň����񂹂�g���K�[
public class GravityTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var character = other.GetComponent<GravityCharacter>();
        if (character != null)
        {
            // �L�����N�^�[�ɏ㏸�J�n���߂��o��
            character.TriggerFloatUp();
        }
    }
}
