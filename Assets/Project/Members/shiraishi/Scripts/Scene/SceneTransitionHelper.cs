using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHelper : MonoBehaviour
{
    public static Texture2D Screenshot;

    public static void StartTransition(string nextSceneName)
    {
        Camera cam = Camera.main; // or ‘JˆÚŒ³‚ÌUI‚ğ•`‰æ‚µ‚Ä‚éƒJƒƒ‰
        int width = Screen.width;
        int height = Screen.height;

        RenderTexture rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        Object.Destroy(rt);

        Screenshot = tex;

        NextSceneName = nextSceneName;

        // TransitionƒV[ƒ“‚ÉˆÚ“®
        SceneManager.LoadScene("TransitionScene");
    }

    public static string NextSceneName;
}
