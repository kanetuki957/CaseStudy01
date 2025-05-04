using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �X�C�b�`�ƂȂ�GameObject�ɃA�^�b�`���A�N���b�N���ɑΏۃI�u�W�F�N�g�𓮂�������
public class SwitchController : MonoBehaviour
{
    // �Ώۂ̃I�u�W�F�N�g
    public GameObject targetObject;

    // ����GameObject���N���b�N���ꂽ�Ƃ��ɌĂ΂�鏈���i�}�E�X����j
    void OnMouseDown()
    {
        // �ΏۃI�u�W�F�N�g���ݒ肳��Ă��邩�m�F
        if (targetObject != null)
        {
            // MoveTarget �X�N���v�g��T��
            var moveScript = targetObject.GetComponent<MoveBeltconveyor>();
            if (moveScript != null)
            {
                // �ΏۃI�u�W�F�N�g���ݒ肳��Ă��邩�m�F
                moveScript.StartMovingLeft();
            }
        }
    }
}