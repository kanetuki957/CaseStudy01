using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHighlightOneTime : MonoBehaviour
{
    // �L�����N�^�[�I�u�W�F�N�g
    public GameObject characterObject;

    // �{�^�������ɕς��F�ݒ�
    public Color highlightColor = Color.magenta;

    // ��x�����������ǂ���
    private bool activated = false;

    // �{�^���̐F��ς��邽�߂�SpriteRenderer
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ��x�ł����������牽�����Ȃ�
        if (activated) return;

        // �L�����N�^�[���^��ɂ��邩�m�F
        if (IsCharacterAbove())
        {
            sr.color = highlightColor;
            activated = true;
        }
    }

    // �L�������{�^���̐^��ɂ��邩�`�F�b�N����֐�
    bool IsCharacterAbove()
    {
        Vector3 charPos = characterObject.transform.position;
        Vector3 btnPos = transform.position;

        return Mathf.Abs(charPos.x - btnPos.x) < 0.5f && charPos.y > btnPos.y;
    }
}
