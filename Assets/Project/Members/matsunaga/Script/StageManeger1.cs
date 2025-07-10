using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static StageManager instance;

    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Resources フォルダから StageManager プレハブを読み込んで生成
                GameObject prefab = Resources.Load<GameObject>("StageManager");
                if (prefab != null)
                {
                    GameObject obj = Instantiate(prefab);
                    instance = obj.GetComponent<StageManager>();
                }
                else
                {
                    Debug.LogError("Resources フォルダに StageManager プレハブが見つかりません。");
                }
            }
            return instance;
        }
    }

    public string previousStageName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ClearStage(string stageName)
    {
        PlayerPrefs.SetInt($"{stageName}_Cleared", 1);
        PlayerPrefs.Save();
    }

    public bool IsStageCleared(string stageName)
    {
        return PlayerPrefs.GetInt($"{stageName}_Cleared", 0) == 1;
    }


    public void MarkPreviousStageAsCleared()
    {
        if (!string.IsNullOrEmpty(previousStageName))
        {
            ClearStage(previousStageName);
            previousStageName = null; // 一度だけ保存
        }
    }


}
