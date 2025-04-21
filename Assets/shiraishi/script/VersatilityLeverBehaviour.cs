using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VersatilityLeverBehaviour : MonoBehaviour
{

    public MonoBehaviour target; // �N������Ώۂ̃X�N���v�g�iIActivatable��z��j


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
                Debug.Log("���o�[���������N�����܂���");
            }
            else
            {
                Debug.LogWarning("target �� IActivatable ����������Ă��܂���");
            }
        }
    }
}
