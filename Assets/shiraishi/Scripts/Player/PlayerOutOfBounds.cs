using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOutOfBounds : MonoBehaviour
{
    [Header("���ɏo����J�ڂ���V�[���i�󗓂Ȃ烊�X�^�[�g�j")]
    public string fallScene;

    [Header("��ɏo����J�ڂ���V�[���i�󗓂Ȃ烊�X�^�[�g�j")]
    public string ascendScene;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);

        // ��ʉ��ɏo����
        if (viewportPos.y < 0f)
        {
            Debug.Log("[PlayerOutOfBounds] ���ɗ�����");
            LoadOrRestart(fallScene);
        }

        // ��ʏ�ɏo����
        if (viewportPos.y > 1f)
        {
            Debug.Log("[PlayerOutOfBounds] ��ɔ�����");
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
