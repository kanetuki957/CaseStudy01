using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersatilityLever : MonoBehaviour
{
    [Header("�N���������I�u�W�F�N�g (IActivatable���p��)")]
    public MonoBehaviour target; // IActivatable���p�������N���X���������I�u�W�F�N�g������

    private Collider2D playerCollider2D;    // �v���C���[�̔���

    [Header("�N���ݒ�")]
    public bool activateOnTouch = false; // true�Ȃ�G�ꂽ�u�ԂɋN��

    [Header("�N���p�L�[ (activateOnTouch=false�̎������L��)")]
    public KeyCode activateKey = KeyCode.F; // �N���p�̃L�[

    private bool hasActivated = false; // �u�G�ꂽ�����N���v�p�̈�񐧌�

    void Update()
    {
        if (!activateOnTouch && playerCollider2D != null && Input.GetKeyDown(activateKey))
        {
            // �L�[�������[�h�̂Ƃ��� hasActivated�������Ă�������
            TryActivate(ignoreHasActivated: true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollider2D = other;

            if (activateOnTouch)
            {
                TryActivate(ignoreHasActivated: false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollider2D = null;
            hasActivated = false; // �G�ꂽ�N���p�������Z�b�g
        }
    }

    private void TryActivate(bool ignoreHasActivated)
    {
        if (!ignoreHasActivated && hasActivated) return;

        if (target is IActivatable activatable)
        {
            activatable.Activate();
            Debug.Log("���o�[���������N�����܂���");

            if (!ignoreHasActivated)
            {
                hasActivated = true; // �u�G�ꂽ�����v�̂Ƃ��������b�N
            }
        }
        else
        {
            Debug.LogWarning("target �� IActivatable ����������Ă��܂���");
        }
    }
}
