using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOutOfBounds : MonoBehaviour
{
    [Header("下に出たら遷移するシーン（空欄ならリスタート）")]
    public string fallScene;

    [Header("上に出たら遷移するシーン（空欄ならリスタート）")]
    public string ascendScene;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);

        // 画面下に出たら
        if (viewportPos.y < 0f)
        {
            Debug.Log("[PlayerOutOfBounds] 下に落ちた");
            LoadOrRestart(fallScene);
        }

        // 画面上に出たら
        if (viewportPos.y > 1f)
        {
            Debug.Log("[PlayerOutOfBounds] 上に抜けた");
            LoadOrRestart(ascendScene);
        }
    }

    private void LoadOrRestart(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
