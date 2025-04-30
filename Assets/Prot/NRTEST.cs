using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NRTEST : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    // ���̏�Ԑ؂�ւ����s���I�u�W�F�N�g���������������m�F����X�N���v�g
    [SerializeField] private NRTEST02 hitTrigger;

    // �R�s�y�ő吔�����
    [SerializeField] private int maxCopySize = 10;

    // �ϊ��O�I�u�W�F�N�g��ݒ�
    [SerializeField] private GameObject beforeObject;
    // �ϊ���I�u�W�F�N�g��ݒ�
    [SerializeField] private GameObject changedObject;


    // �����I�u�W�F�N�g(�ϊ��O)��ۑ�
    // �����ɕω��O�̋�������
    // �Z�b�g��������BridgeManager�̎q�I�u�W�F�N�g�ɂ���
    [SerializeField] private List<GameObject> beforeObjectList = new List<GameObject>();
    // �����I�u�W�F�N�g(�ϊ���)��ۑ�
    List<GameObject> changedObjectList = new List<GameObject>();


    // �I�����Ă���I�u�W�F�N�g��ۑ�
    private GameObject selectObject;



    private void Start()
    {
        // ���O�ɃI�u�W�F�N�g�𐶐�
        for(int i = 0; i< maxCopySize; i++)
        {
            if (beforeObjectList.Count < maxCopySize)
            {
                // �ω��O�I�u�W�F�N�g���쐬
                GameObject newBeforeObj = Instantiate(beforeObject, transform);
                newBeforeObj.SetActive(false);
                beforeObjectList.Add(newBeforeObj);
            }

            // �ω���I�u�W�F�N�g���쐬
            GameObject newChangedObj = Instantiate(changedObject, transform);
            newChangedObj.SetActive(false);
            changedObjectList.Add(newChangedObj);
        }

        //  ��ڂ̃I�u�W�F�N�g��\�����Ă���
        beforeObjectList[0].SetActive(true);
    }


    private void Update()
    {
        Copy();
        Paste();

        // �n�[�g�ɓ���������
        if(hitTrigger.isHit)
        {
            for(int i = 0; i < beforeObjectList.Count; i++)
            {
                // �ω��O�I�u�W�F�N�g�B�ŃA�N�e�B�u�Ȃ��̂�T��
                if (beforeObjectList[i].activeInHierarchy)
                {
                    beforeObjectList[i].SetActive(false);

                    // �ω���I�u�W�F�N�g���A�N�e�B�u��
                    changedObjectList[i].SetActive(true);
                    // ���W��ݒ�
                    changedObjectList[i].transform.position = beforeObjectList[i].transform.position;

                }
            }

            // ������ false �ɍĐݒ�����Ȃ��Ɖi���ɔ��������Ⴄ
            hitTrigger.ResetHit();
        }
    }



    void Copy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = GetCameraUnderMouse();
            if (cam == null) return;

            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                selectObject = hit.collider.gameObject;
                Debug.Log("Selected: " + selectObject.name);
            }
        }
    }


    void Paste()
    {
        if (selectObject.CompareTag("Finish") && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
        {
            Camera cam = GetCameraUnderMouse();
            if (cam == null) return;

            // �ύX��������ۑ�
            for (int i = 0; i < beforeObjectList.Count; i++)
            {
                // ��A�N�e�B�u�̃I�u�W�F�N�g����������
                if (!beforeObjectList[i].activeInHierarchy)
                {
                    // �A�N�e�B�u�ɕύX
                    beforeObjectList[i].SetActive(true);
                    Vector2 spawnPos = cam.ScreenToWorldPoint(Input.mousePosition);

                    beforeObjectList[i].transform.position = spawnPos;
                    // �ύX������I��
                    break;
                }
            }
        }
    }



    Camera GetCameraUnderMouse()
    {
        if (IsMouseInCamera(camera1)) return camera1;
        if (IsMouseInCamera(camera2)) return camera2;
        return null;
    }

    bool IsMouseInCamera(Camera cam)
    {
        Vector3 viewPortPos = cam.ScreenToViewportPoint(Input.mousePosition);
        return viewPortPos.x >= 0 && viewPortPos.x <= 1 && viewPortPos.y >= 0 && viewPortPos.y <= 1;
    }



}
