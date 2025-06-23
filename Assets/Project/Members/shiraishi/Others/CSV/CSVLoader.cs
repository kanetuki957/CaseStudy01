using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GimmickPair
{
    public int id;
    public GameObject prefab;
}

public class CSVLoader : MonoBehaviour
{
    public TextAsset csvFile;
    public List<GimmickPair> gimmickPrefabs; // Inspectorで番号とPrefabを紐付け
    private Dictionary<int, GameObject> idToPrefab;

#if UNITY_EDITOR
    public void GenerateInEditor()
    {
        // すでに配置済みの子を全部消す
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        idToPrefab = new Dictionary<int, GameObject>();
        foreach (var pair in gimmickPrefabs)
            idToPrefab[pair.id] = pair.prefab;


        string[] lines = csvFile.text.Split('\n');
        List<string> lineList = new List<string>();
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line.Trim()))  // ← 空行は除外
                lineList.Add(line.Trim());
        }

        int rowCount = lineList.Count;
        int colCount = 0;

        foreach (var line in lines)
        {
            int c = line.Trim().Split(',').Length;
            if (c > colCount) colCount = c;
        }

        float cellSize = 1f; // 1マスのサイズ（Prefabサイズが1ならこれでOK）

        // 中央揃え配置
        for (int y = 0; y < rowCount; y++)
        {
            string[] cells = lines[y].Trim().Split(',');
            for (int x = 0; x < cells.Length; x++)
            {
                string cell = cells[x].Trim();
                // マップの中心をワールド原点(0,0)に合わせる
                float px = (x - colCount / 2f + 0.5f) * cellSize;
                float py = -(y - rowCount / 2f + 0.5f) * cellSize;
                Vector3 pos = new Vector3(px, py, 0);

                int id;
                if (int.TryParse(cells[x], out id) && idToPrefab.ContainsKey(id))
                {

                    GameObject prefab = idToPrefab[id];
                    if (prefab != null)
                    {
                        var go = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, this.transform);
                        go.transform.localPosition = pos;
                    }
                }
            }
        }

        AdjustCameraToMap_CenterOrigin(colCount, rowCount, cellSize);
    }

    void AdjustCameraToMap_CenterOrigin(int colCount, int rowCount, float cellSize = 1f)
    {
        float aspect = 16f / 9f;
        float mapWidth = colCount * cellSize;
        float mapHeight = rowCount * cellSize;

        float orthoSize = Mathf.Max(mapHeight / 2f, mapWidth / 2f / aspect);

        // カメラを原点(0,0)に
        Camera.main.orthographicSize = orthoSize;
        Camera.main.transform.position = new Vector3(0, 0, -10f);

        // デバッグ
        Debug.Log($"===カメラ調整===");
        Debug.Log($"col={colCount}, row={rowCount}, cellSize={cellSize}");
        Debug.Log($"mapWidth={mapWidth}, mapHeight={mapHeight}");
        Debug.Log($"Camera center=(0,0), orthoSize={orthoSize}");
    }
#endif
}
