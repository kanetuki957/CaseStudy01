using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    public replace targetBridge;

    private bool alreadyTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!alreadyTriggered && other.CompareTag("Player"))
        {
            alreadyTriggered = true;

            if (targetBridge != null)
            {
                // ���̋����J��
                targetBridge.TriggerOpen();
                // �V�[����̑S�R�s�[��T���Ĉꏏ�ɊJ��
                OpenAllCopiedBridges();
            }
            else
            {
                Debug.LogWarning("HeartTrigger�Freplace���ݒ肳��Ă��܂���I");
            }
        }
    }

    void OpenAllCopiedBridges()
    {
        // ���Ɠ������O�� "_Copy" ��������S���T��
        string targetName = targetBridge.gameObject.name;

        replace[] allBridges = FindObjectsOfType<replace>();
        foreach (replace r in allBridges)
        {
            if (r != targetBridge && r.gameObject.name.StartsWith(targetName + "_Copy"))
            {
                r.TriggerOpen();
            }
        }
    }

}
