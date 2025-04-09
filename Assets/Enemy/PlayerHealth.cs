using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // �V���O���g���p�^�[���i���̃X�N���v�g���� `GameDirector.Instance` �ŃA�N�Z�X�ł���j
    public static PlayerHealth Instance;

    [SerializeField]
    private GameObject[] textureObjects; // ���ԂɕύX����I�u�W�F�N�g�̔z��

    [SerializeField]
    private Sprite[] textureStages; // �ύX����e�N�X�`���̔z��
    private int currentStage = 0;   // ���݂̃I�u�W�F�N�g�C���f�b�N�X�i�ǂ̃I�u�W�F�N�g��ύX���邩�Ǘ��j
    public float scaleMultiplier;   // �X�P�[���̊g�嗦�i�ύX��̃T�C�Y�����߂�j

    void Awake()
    {
        // �V���O���g���̐ݒ�i���ɑ��݂���ꍇ�͍폜�j
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �v���C���[���̃e�N�X�`�����w��񐔕ύX����֐�
    public void ChangePlayerTexture(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            // �e�N�X�`�����܂��c���Ă���ꍇ�̂ݕύX
            if (currentStage < textureObjects.Length)
            {
                GameObject obj = textureObjects[currentStage];

                if (obj != null)
                {
                    // �X�v���C�g�����_���[���擾���A�e�N�X�`���ύX
                    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.sprite = textureStages[currentStage];    // �e�N�X�`�����X�V
                        obj.transform.localScale *= scaleMultiplier;// �X�P�[�����g��
                    }
                    currentStage++;// ���̒i�K��
                }
            }
        }
    }
}