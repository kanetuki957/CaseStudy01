using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // �V���O���g���p�^�[���i���̃X�N���v�g���� `GameDirector.Instance` �ŃA�N�Z�X�ł���j
    public static PlayerHealth Instance;

    [SerializeField]
    private GameObject[] textureObjects; // **���ԂɕύX����I�u�W�F�N�g�̔z��**

    [SerializeField]
    private Sprite[] textureStages; // **�ύX����e�N�X�`���̔z��**

    private int currentStage = 0; // **���݂̃I�u�W�F�N�g�C���f�b�N�X�i�ǂ̃I�u�W�F�N�g��ύX���邩�Ǘ��j**
    public float scaleMultiplier; // **�X�P�[���̊g�嗦�i�ύX��̃T�C�Y�����߂�j**

    void Awake()
    {
        // **�V���O���g���̐ݒ�i���ɑ��݂���ꍇ�͍폜�j**
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // **1��̏Փ˂��Ƃ�1�̃I�u�W�F�N�g�̃e�N�X�`����ύX����֐�**
    public void ChangeTexture()
    {
        // **�܂��ύX�ł���I�u�W�F�N�g������ꍇ�̂ݎ��s**
        if (currentStage < textureObjects.Length)
        {
            GameObject obj = textureObjects[currentStage]; // **���ɕύX����I�u�W�F�N�g���擾**

            if (obj != null)
            {
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = textureStages[currentStage]; // **�Ή�����e�N�X�`���ɕύX**

                    // **�X�P�[�����g��i���̃T�C�Y �~ scaleMultiplier�j**
                    obj.transform.localScale *= scaleMultiplier;
                }

                currentStage++; // **���̃I�u�W�F�N�g�֐i�߂�**
            }
        }
    }
}