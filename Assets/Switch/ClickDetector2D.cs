using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetector2D : MonoBehaviour
{
    public GameObject player; // �v���C���[�I�u�W�F�N�g�iInspector �Ŏw��j

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // �}�E�X�ʒu�����[���h���W�ɕϊ�
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Raycast�łQ�c�I�u�W�F�N�g�𔻒�
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                // ���O�Ŕ���i�܂��̓X�N���v�g�Ŕ���j
                if (hit.collider.gameObject.name == "Switch")
                {
                    player.GetComponent<PlayerMover2D>().ToggleMove();
                }
            }
        }
    }
}
