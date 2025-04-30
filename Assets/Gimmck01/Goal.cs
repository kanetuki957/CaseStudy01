using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI���g�����߂ɕK�v

public class Goal : MonoBehaviour
{
    public string main_goal; // �J�ڐ�̃V�[������Inspector�Ŏw��
    private GameObject player; // �v���C���[�I�u�W�F�N�g���i�[����ϐ�

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // �v���C���[�I�u�W�F�N�g���擾
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �v���C���[���G�ꂽ��
        {
            if (GameObject.Find("key")) // �V�[�����ɃL�[������ꍇ
            {
                if (player.GetComponent<aitem>().collectedItems.Contains("key"))    // �v���C���[���擾�����A�C�e�����ɃL�[������Ȃ�
                {
                    SceneManager.LoadScene(main_goal); // �w�肵���V�[���ֈړ�
                }
            }
            else
                SceneManager.LoadScene(main_goal); // �w�肵���V�[���ֈړ�
        }
    }
}
