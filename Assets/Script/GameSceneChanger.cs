using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneChanger : MonoBehaviour
{
    public Button changeSceneButton;
    public string sceneName;

    void Start()
    {
        if (changeSceneButton != null)
        {
            changeSceneButton.onClick.AddListener(() => ChangeScene(sceneName));
        }
        else
        {
            Debug.LogError("Button���ݒ肳��Ă��܂���B");
        }
    }

    public void ChangeScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadSceneAsync(sceneName);
            Debug.Log("�V�[�����ύX����܂���: " + sceneName);
        }
        else
        {
            Debug.LogError("�V�[�������ݒ肳��Ă��܂���B");
        }
    }
}