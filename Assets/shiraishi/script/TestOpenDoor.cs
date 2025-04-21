using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenDoor : MonoBehaviour, IActivatable
{
    public GameObject door;  // ����GameObject
    public Sprite openDoorSprite; // �J�������̉摜
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
        if (doorSpriteRenderer != null && openDoorSprite != null)
        {
            doorSpriteRenderer.sprite = openDoorSprite; // ���̉摜���J������ԂɕύX
        }

        if (doorCollider != null)
        {
            Destroy(doorCollider); // ���̃R���C�_�[���폜
        }
    }
}
