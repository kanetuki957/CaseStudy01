using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHighlightWhileAbove : MonoBehaviour
{
    // �L�����N�^�[�I�u�W�F�N�g
    public GameObject characterObject;

    // �ʏ�̐F�i�^��ɂ��Ȃ��Ƃ��j
    public Color normalColor = Color.white;

    // �^��ɂ���Ƃ��̎��F
    public Color highlightColor = Color.magenta;

    // �F��؂�ւ��邽�߂�SpriteRenderer
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // �^��ɂ���Ƃ��������ɁA����ȊO�͌��̐F��
        if (IsCharacterAbove())
        {
            sr.color = highlightColor;
        }
        else
        {
            sr.color = normalColor;
        }
    }

    // �L�������^��ɂ��邩�𔻒肷��
    bool IsCharacterAbove()
    {
        Vector3 charPos = characterObject.transform.position;
        Vector3 btnPos = transform.position;

        return Mathf.Abs(charPos.x - btnPos.x) < 0.5f && charPos.y > btnPos.y;
    }
}
