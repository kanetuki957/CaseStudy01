using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPortal : MonoBehaviour
{
    public GameObject redWarpTarget;
    public GameObject blueWarpTarget;
    public GameObject purpleWarpTarget;

    private bool isWarping = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isWarping) return;

        string objName = other.gameObject.name;
        GameObject target = null;

        if (objName.Contains("redwarp1") && redWarpTarget != null)
            target = redWarpTarget;
        else if (objName.Contains("bluewarp2") && blueWarpTarget != null)
            target = blueWarpTarget;
        else if (objName.Contains("purplewarp1") && purpleWarpTarget != null)
            target = purpleWarpTarget;

        if (target != null)
        {
            StartCoroutine(WarpTo(target));
        }
    }

    IEnumerator WarpTo(GameObject target)
    {
        isWarping = true;
        transform.position = target.transform.position;
        yield return new WaitForSeconds(0.5f);
        isWarping = false;
    }
}