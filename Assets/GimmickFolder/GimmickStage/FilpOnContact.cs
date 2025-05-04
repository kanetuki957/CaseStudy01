using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �y�L�����N�^�[������I�u�W�F�N�g�ɐG�ꂽ��A�w�肵���I�u�W�F�N�g�Q���㉺���]������X�N���v�g�z
public class FilpOnContact : MonoBehaviour
{
    public GameObject targetObject;           // �Ԃ���Ώ�
    public List<GameObject> objectsToReverse; // ���]���������I�u�W�F�N�g���X�g

    private bool hasReversed = false;         // 1�񂾂����]���邽�߂̃t���O

    // ���̃I�u�W�F�N�g�ƂԂ������Ƃ��ɌĂ΂��֐�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �܂����]���Ă��Ȃ��āA�^�[�Q�b�g�I�u�W�F�N�g�ɂԂ�������
        if (!hasReversed && collision.gameObject == targetObject)
        {
            foreach (GameObject obj in objectsToReverse)
            {
                if (obj != null)
                {
                    Vector3 scale = obj.transform.localScale; // ���݂̃X�P�[�����擾
                    scale.y *= -1;                            // �㉺�������]����
                    obj.transform.localScale = scale;         // �V�����X�P�[����K�p
                }
            }

            hasReversed = true; // �������]�ς݂��ƃ}�[�N����i2��ڈȍ~�͖�������j
        }
    }
}
