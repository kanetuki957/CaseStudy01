using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloorOnClick : MonoBehaviour
{
    // �������Ώۂ̏��I�u�W�F�N�g
    public GameObject floorObject;

    // �L�����N�^�[�i�ړ������ǂ����𔻒肷��j
    public GameObject characterObject;

    // �����ǂꂾ����ɓ�����
    public float moveDistance = 2f;

    // ������������
    public float moveDuration = 1f;

    // 1�񂾂������悤�ɂ���t���O
    private bool hasMoved = false;

    // �I�u�W�F�N�g���N���b�N���ꂽ�Ƃ��̏���
    void OnMouseDown()
    {
        // ���łɓ��������疳��
        if (hasMoved) return;

        // �L�����N�^�[�������Ă���ꍇ�������s
        var rb = characterObject.GetComponent<Rigidbody2D>();
        if (rb != null && Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            hasMoved = true;
            StartCoroutine(MoveFloorSmoothly());
        }
    }

    // �����Ȃ߂炩�ɏ�ɓ������R���[�`������
    System.Collections.IEnumerator MoveFloorSmoothly()
    {
        Vector3 startPos = floorObject.transform.position;
        Vector3 endPos = startPos + new Vector3(0f, moveDistance, 0f);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            floorObject.transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // �Ō�ɐ��m�Ȉʒu�ɍ��킹�ďI��
        floorObject.transform.position = endPos;
    }
}
