using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VersatilityLever : MonoBehaviour
{
    [Header("�N���������I�u�W�F�N�g(IActivatable���p��)")]
    public MonoBehaviour target; // IActivatable���p�������N���X���������I�u�W�F�N�g������

    private Collider2D playerCollider2D;    // �v���C���[�̔���

    [Header("�N���p�L�[")]
    public KeyCode activateKey = KeyCode.F; // �N���p�̃L�[


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �L�[�����͂����@���@�v���C���[�����o�[�ɏd�Ȃ��Ă���
        if (Input.GetKeyDown(activateKey) && playerCollider2D != null)
        {
            // �^�[�Q�b�g�̃I�u�W�F�N�g��IActivatable���p�������N���X������
            if (target is IActivatable activatable)
            {
                // �^�[�Q�b�g�������Ă���Activate�����s
                activatable.Activate();

                Debug.Log("���o�[���������N�����܂���");
            }
            else
            {
                Debug.LogWarning("target �� IActivatable ����������Ă��܂���");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollider2D = other;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollider2D = null;
        }
    }
}
