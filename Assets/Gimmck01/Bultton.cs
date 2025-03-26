using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bultton : MonoBehaviour
{
    public GameObject door;  // ����GameObject
    public Sprite openDoorSprite; // �J�������̉摜
    private SpriteRenderer doorSpriteRenderer;
    private BoxCollider2D doorCollider;
    private Vector3 originalPosition;
    private bool isPressed = false;

    void Start()
    {
        originalPosition = transform.position; // �����ʒu���L��

        if (door != null)
        {
            doorSpriteRenderer = door.GetComponent<SpriteRenderer>(); // ���̃X�v���C�g�����_���[���擾
            doorCollider = door.GetComponent<BoxCollider2D>(); // ���̃R���C�_�[���擾
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed && other.CompareTag("Player")) // �v���C���[�����񂾂�쓮
        {
            isPressed = true;
            transform.position -= new Vector3(0, 0.3f, 0); // �{�^���𒾂߂�
            OpenDoor(); // �����J���i�摜�ύX�{�R���C�_�[�폜�j
        }
    }

    private void OpenDoor()
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
