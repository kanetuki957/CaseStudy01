using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMover : MonoBehaviour
{
    // �v���C���[�̈ړ����x
    public float speed = 2f;

    // �����^���̈ړ������i��ʒ[�Ŕ��]���Ė߂鋗���j
    public float moveDistance = 5f;

    // ���ׂẴA�C�e���擾��A�ŏ��Ɍ������E�[��X���W
    public float moveToRightEdgeX = 10f;

    // �A�C�e���擾����\������ TextMeshPro UI �e�L�X�g
    public TMP_Text countText;

    // �A�C�e���Ƃ��Ĉ����I�u�W�F�N�g���Ɋ܂܂�镶����
    public string itemKeyword = "Item";

    // �擾�Ώۂ̃A�C�e���I�u�W�F�N�g�ꗗ�i���O�Ńt�B���^�ς݁j
    private List<GameObject> itemList = new List<GameObject>();

    // ���݂܂łɎ擾�����A�C�e����
    private int collectedCount = 0;

    // �S�ẴA�C�e�����擾���I��������ǂ����̃t���O
    private bool allCollected = false;

    // �E�[�ɓ��B���A�����^�����J�n���Ă��邩�ǂ����̃t���O
    private bool startPingPong = false;

    // �����ړ��̋N�_�ƂȂ�ʒu
    private Vector3 startPosition;

    // ���݂̈ړ������i1 = �E�A-1 = ���j
    private int direction = 1;

    // �����������i�A�C�e�����o��UI�����\���j
    void Start()
    {
        // �V�[�����ɑ��݂��邷�ׂẴI�u�W�F�N�g�𑖍�
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // �I�u�W�F�N�g����item���܂܂�Ă�����̂��A�C�e���Ƃ��ēo�^
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains(itemKeyword))
            {
                itemList.Add(obj);
            }
        }

        // UI�e�L�X�g�������\���i0�j
        UpdateText();

        // �ŏ��̈ړ��J�n�n�_���L�^
        startPosition = transform.position;
    }

    // ���t���[���Ă΂�鏈��
    void Update()
    {
        if (!allCollected)
        {
            // �A�C�e�����擾���F��ɉE�Ɉړ�
            transform.position += Vector3.right * speed * Time.deltaTime;

            // �A�C�e����S�Ď擾������t���O���X�V
            if (itemList.Count == 0)
            {
                allCollected = true;
            }
        }
        else if (!startPingPong)
        {
            // �S�擾��F��x�����E�[�imoveToRightEdgeX�j�܂ňړ�
            transform.position += Vector3.right * speed * Time.deltaTime;

            if (transform.position.x >= moveToRightEdgeX)
            {
                // �E�[�ɓ��B �� �����^�����[�h�Ɉڍs
                startPosition = transform.position;
                direction = -1; // ���ֈړ��J�n
                startPingPong = true;
            }
        }
        else
        {
            // �����ړ����[�h�imoveDistance�͈̔͂ō��E�Ɉړ��j
            transform.position += Vector3.right * direction * speed * Time.deltaTime;

            // ��苗���i�񂾂�ړ������𔽓]
            if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            {
                direction *= -1;                    // ���]�i1 ? -1�j
                startPosition = transform.position; // ���݈ʒu��V���ȋN�_��
            }
        }
    }

    // �A�C�e���Ƃ̏Փ˂����o����֐�
    void OnTriggerEnter2D(Collider2D other)
    {
        // ���O��item���܂ނ��̂������A�C�e���Ɣ���
        if (other.gameObject.name.Contains(itemKeyword))
        {
            collectedCount++;                 // �J�E���g���Z
            UpdateText();                     // UI�X�V
            Destroy(other.gameObject);        // �A�C�e�����폜
            itemList.Remove(other.gameObject);// ���X�g������폜
        }
    }

    // UI�e�L�X�g�Ɍ��݂̃J�E���g�𔽉f����֐�
    void UpdateText()
    {
        countText.text = collectedCount.ToString();
    }
}