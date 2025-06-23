using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAnimContoroll : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("Fall"); 
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("Idle");
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetTrigger("Jump");
        }
    }
}
