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
                // 元の橋を開く
                targetBridge.TriggerOpen();
                // シーン上の全コピーを探して一緒に開く
                OpenAllCopiedBridges();
            }
            else
            {
                Debug.LogWarning("HeartTrigger：replaceが設定されていません！");
            }
        }
    }

    void OpenAllCopiedBridges()
    {
        // 橋と同じ名前で "_Copy" がついたやつ全部探す
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
