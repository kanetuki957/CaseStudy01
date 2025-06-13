using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVLoader : MonoBehaviour
{
    public TextAsset csvFile;

    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject goalPrefab;


#if UNITY_EDITOR
    public void GenerateInEditor()
    {
        // すでに配置済みの子を全部消す（安全対策）
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        string[] lines = csvFile.text.Split('\n');
        int rowCount = lines.Length;
        int colCount = 0;
        foreach (var line in lines)
        {
            int c = line.Trim().Split(',').Length;
            if (c > colCount) colCount = c;
        }

        float offsetX = (colCount - 1) / 2.0f;
        float offsetY = (rowCount - 1) / 2.0f;

        for (int y = 0; y < rowCount; y++)
        {
            string[] cells = lines[y].Trim().Split(',');
            for (int x = 0; x < cells.Length; x++)
            {
                string cell = cells[x].Trim();
                Vector3 pos = new Vector3(x - offsetX, -(y - offsetY), 0);

                GameObject prefab = null;
                if (cell == "1") prefab = wallPrefab;
                else if (cell == "P") prefab = playerPrefab;
                else if (cell == "G") prefab = goalPrefab;
                if (prefab != null)
                {
                    GameObject go = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, this.transform);
                    go.transform.localPosition = pos;
                }
            }
        }

        AdjustCameraToMap(colCount, rowCount);
    }
    void AdjustCameraToMap(int colCount, int rowCount)
    {

        Debug.Log(colCount + "+" + rowCount);
        float aspect = (float)Screen.width / Screen.height;
        float mapWidth = colCount;
        float mapHeight = rowCount;

        float sizeY = mapHeight / 2f;
        float sizeX = mapWidth / 2f / aspect;
        float orthoSize = Mathf.Max(sizeY, sizeX);

        Camera.main.orthographicSize = orthoSize;

        // ★ここ！0.5分だけズラす
        float mapCenterX = (colCount - 1) / 2f;
        float mapCenterY = -(rowCount - 1) / 2f;
        Camera.main.transform.position = new Vector3(mapCenterX, mapCenterY, -10f);
    }
#endif
}
