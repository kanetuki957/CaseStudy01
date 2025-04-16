using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̃X�N���v�g�́A�N���b�N���ꂽ�I�u�W�F�N�g�i���O�� "Switch"�j�ɉ�����
// �Q�̂̃v���C���[�I�u�W�F�N�g�̓���𓯎��ɐ؂�ւ��邽�߂̐�����s���B

public class ClickDetector_Multi : MonoBehaviour
{
    public GameObject player;  // �v���C���[�I�u�W�F�N�g�iInspector �Ŏw��j
    public GameObject player2; // �v���C���[�I�u�W�F�N�g�Q�iInspector �Ŏw��j

    void Update()
    {
        // �}�E�X�̍��N���b�N�����o�i���t���[���j
        if (Input.GetMouseButtonDown(0))
        {
            // �N���b�N���ꂽ��ʏ�̍��W�����[���h���W�ɕϊ�
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // �N���b�N���ꂽ��ʏ�̍��W�����[���h���W�ɕϊ�
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                // �N���b�N���ꂽ�I�u�W�F�N�g�̖��O�� "Switch" ���ǂ������m�F
                // ���O�Ŕ��肷�邱�ƂŁA����̃I�u�W�F�N�g�ɂ̂ݔ���������
                if (hit.collider.gameObject.name == "Switch")
                {
                    // 2�̂̃v���C���[�̓���𓯎��ɐ؂�ւ���
                    // ToggleMove() �́A�ړ��J�n ? ��~�{�����ʒu�ɖ߂� ��؂�ւ���
                    player.GetComponent<PlayerMover2D>().ToggleMove();
                    player2.GetComponent<PlayerMover2D>().ToggleMove();
                }
            }
        }
    }
}
