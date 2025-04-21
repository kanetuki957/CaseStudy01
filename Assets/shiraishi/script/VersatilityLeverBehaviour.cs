using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VersatilityLeverBehaviour : MonoBehaviour
{

    public MonoBehaviour target; // 起動する対象のスクリプト（IActivatableを想定）


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (target is IActivatable activatable)
            {
                activatable.Activate();
                Debug.Log("レバーが処理を起動しました");
            }
            else
            {
                Debug.LogWarning("target に IActivatable が実装されていません");
            }
        }
    }
}
