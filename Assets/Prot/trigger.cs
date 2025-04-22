using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    [Tooltip("‹´‚ÌBridgeControllerQÆ")]
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
                Debug.LogWarning("HeartTriggerFreplace‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñI");
            }
        }
    }

}
