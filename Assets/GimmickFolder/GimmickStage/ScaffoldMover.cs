using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldMover : MonoBehaviour
{
    private Vector2 pos;
    public int num = 1;

    public float leftLimit = -3f;  // ���[
    public float rightLimit = 3f;  // �E�[

    void Update()
    {
        pos = transform.position;

        // �ړ�����
        transform.Translate(transform.right * Time.deltaTime * 3 * num);

        // �͈̓`�F�b�N
        if (pos.x > rightLimit)
        {
            num = -1; // ����
        }
        if (pos.x < leftLimit)
        {
            num = 1;  // �E��
        }
    }

}
