using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI���g�����߂ɕK�v

public class Goal : MonoBehaviour
{
    public string main_goal; // �J�ڐ�̃V�[������Inspector�Ŏw��

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �v���C���[���G�ꂽ��
        {
            SceneManager.LoadScene(main_goal); // �w�肵���V�[���ֈړ�
        }
    }
}
