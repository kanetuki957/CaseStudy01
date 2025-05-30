using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public AudioClip selectSE;  // 再生するSE
    private AudioSource audioSource;
    private bool isSelected = false;  // 一度だけ再生するためのフラグ

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnMouseDown()  // オブジェクトがクリックされたとき
    {
        if (!isSelected)
        {
            audioSource.PlayOneShot(selectSE);
            isSelected = true;  // 一度だけ再生
        }

        // 他の選択処理があればここに追加
    }
}
