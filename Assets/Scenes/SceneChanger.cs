using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
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
            Logger.LogError("Buttonが設定されていません。");
        }
    }

    public void ChangeScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadSceneAsync(sceneName);
            Logger.Log("シーンが変更されました: " + sceneName);
        }
        else
        {
            Logger.LogError("シーン名が設定されていません。");
        }
    }
}