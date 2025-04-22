using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    [Tooltip("����BridgeController�Q��")]
    public replace targetBridge;

    private bool alreadyTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!alreadyTriggered && other.CompareTag("Player"))
        {
            alreadyTriggered = true;

            if (targetBridge != null)
            {
                targetBridge.TriggerOpen();
            }
            else
            {
                Debug.LogWarning("HeartTrigger�Freplace���ݒ肳��Ă��܂���I");
            }
        }
    }

}
