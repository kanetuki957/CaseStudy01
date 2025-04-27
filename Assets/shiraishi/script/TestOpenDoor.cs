using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenDoor : MonoBehaviour, IActivatable
{
    public GameObject door;  // ����GameObject
    public Sprite openDoorSprite; // �J�������̉摜
    public Sprite closeDoorSprite;
    private SpriteRenderer doorSpriteRenderer;
    private BoxCollider2D doorCollider;

    void Start()
    {
        if (door != null)
        {
            doorSpriteRenderer = door.GetComponent<SpriteRenderer>(); // ���̃X�v���C�g�����_���[���擾
            doorCollider = door.GetComponent<BoxCollider2D>(); // ���̃R���C�_�[���擾
        }
    }

    public void Activate()
    {
        if (doorCollider.enabled)   // �������Ă���Ȃ�
        {
            doorSpriteRenderer.sprite = openDoorSprite; // ���̉摜���J������ԂɕύX

            doorCollider.enabled = false; // ���̃R���C�_�[�𖳌���
            if (doorCollider.enabled == true)
            {
            }
        }
        else
        {
            doorSpriteRenderer.sprite = closeDoorSprite; // ���̉摜�������ԂɕύX

            doorCollider.enabled = true; // ���̃R���C�_�[��L����
            if (doorCollider.enabled == false)
            {
            }
        }
    }
}
