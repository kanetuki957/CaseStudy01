using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlock : MonoBehaviour
{
    GameObject copiedBlock;
    GameObject movingBlock;
    public int maxcopy;
    private int copynumber;
    public int maxpaste;
    private int pastenumber;
    public float speed = 20f;

    private void Start()
    {
        copynumber = 0;
        pastenumber = 0;
    }




    void Update()
    {
        if (maxcopy > copynumber && Input.GetKeyDown(KeyCode.C))
        {
            CopyBlock();
        }
        if (maxpaste > pastenumber && Input.GetKeyDown(KeyCode.V))
        {
            PasteBlock();
        }
        moveBlock();
    }


    void CopyBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.GetComponent<BoxCollider>() != null)
            {
                copiedBlock = hit.collider.gameObject;
                Debug.Log("�u���b�N���R�s�[���܂����I");
                copynumber++;
            }
        }
    }

    void PasteBlock()
    {
        if (copiedBlock != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 pastePosition = Vector3.zero;
            Vector3 blockSize = copiedBlock.GetComponent<Renderer>().bounds.size; // �u���b�N�T�C�Y���擾

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                pastePosition = hit.point;
            }
            else
            {
                pastePosition = Camera.main.transform.position + Camera.main.transform.forward * 3f;
            }

            // **�Փ˔���iOverlapBox���g�p�j**
            if (Physics.OverlapBox(pastePosition, blockSize / 2.0f).Length == 0)
            {
                Instantiate(copiedBlock, pastePosition, copiedBlock.transform.rotation);
                Debug.Log("�u���b�N���y�[�X�g���܂����I");
                pastenumber++;
            }
            else
            {
                Debug.Log("�y�[�X�g�ł��܂���I���Ƀu���b�N������܂��B");
            }
        }
    }  
    void moveBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.GetComponent<CreateBlock>() != null)
            {
                movingBlock = hit.collider.gameObject;
              
            }
            
        }
        if (Input.GetMouseButton(1)) // ���N���b�N�������ꑱ���Ă��邩�`�F�b�N
        {
            Vector3 mousePos = Input.mousePosition; // �}�E�X�̉�ʏ�̈ʒu���擾
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f)); // ���[���h���W�ɕϊ�

            // ���̃u���b�N�Ƃ̏d�Ȃ�𔻒�
            Collider2D hit1 = Physics2D.OverlapBox(mousePos, transform.localScale, 0);
            if (hit1 == null) // �����Ȃ��ꏊ�Ȃ�ړ�
            {
                // �I�u�W�F�N�g���}�E�X�̈ʒu�Ɉړ��i�X���[�Y�ɂ��邽�߂�Lerp�g�p�j
                movingBlock.transform.position = Vector3.Lerp(movingBlock.transform.position, mousePos, speed * Time.deltaTime);

            }

        }

    }
    public void PlusCopy(int copy)
    {
        maxcopy = maxcopy + copy;
        Debug.Log(maxcopy);
    }
    public void MinusCopy(int copy)
    {
        maxcopy = maxcopy - copy;
        Debug.Log(maxcopy);
    }


}