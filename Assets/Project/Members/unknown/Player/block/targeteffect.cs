using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEffect : MonoBehaviour
{
    public bool isInside = false; // ”ÍˆÍ“à‚©‚Ç‚¤‚©‚ğŠÇ—‚·‚é•Ï”

    private void Update()
    {
        if (isInside == true)
        {
            Destroy(gameObject);
        }
    }

}
