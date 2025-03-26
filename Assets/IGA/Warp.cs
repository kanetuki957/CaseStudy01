using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField] private Vector3 warpPosition; // ���[�v��̈ʒu

    private bool isPlayerInside = false;  // �v���C���[���y�ǂ̒��ɂ��邩�ǂ���
    private GameObject player;          // �v���C���[�̃I�u�W�F�N�g�ێ�

    // �v���C���[�����[�v�ɐG��Ă���Ƃ�
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true; // ���[�v���Ă����Ԃɂ���
            player = other.gameObject; // �v���C���[�̃I�u�W�F�N�g���擾
        }
    }

    // �v���C���[�����[�v�]�[������o���Ƃ�
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false; // ���[�v�s��Ԃɂ���
            player = null; // �v���C���[�������Z�b�g
        }
    }

    // ���t���[���`�F�b�N
    private void Update()
    {
        // �v���C���[���]�[�����ɂ��āA���[�v�L�[�������ꂽ�烏�[�v
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F�L�[�������ꂽ�I");
        }

        if (isPlayerInside && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("���[�v�������Ă΂ꂽ�I");
            WarpPlayer();
        }
    }

    // �v���C���[�����[�v������
    private void WarpPlayer()
    {
        if (player != null)
        {
            Debug.Log("���[�v�J�n�I���݂̈ʒu: " + player.transform.position);
            player.transform.position = warpPosition; // ���[�v
            Debug.Log("���[�v�����I�V�����ʒu: " + player.transform.position);
            player.transform.position = warpPosition; // �v���C���[�̈ʒu�����[�v��ɕύX
        }
    }
}
