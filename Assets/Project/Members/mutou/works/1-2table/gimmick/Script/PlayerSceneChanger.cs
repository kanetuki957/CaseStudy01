using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneChanger : MonoBehaviour
{
    public string targetSceneName;
    public GameObject sceneChangeTrigger;

    private bool sceneChanging = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!sceneChanging && other.gameObject == sceneChangeTrigger)
        {
            sceneChanging = true;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}