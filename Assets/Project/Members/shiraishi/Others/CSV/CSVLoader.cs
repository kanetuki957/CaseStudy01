using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GimmickPair
{
    public int id;
    public GameObject prefab;
    public int layer = 0; // InspectorでLayer番号も指定できる！
}

public class CSVLoader : MonoBehaviour
{
    public TextAsset csvFile;
    public List<GimmickPair> gimmickPrefabs; // Inspectorで番号・Prefab・Layer紐付け
    private Dictionary<int, GimmickPair> idToPair; // PrefabだけでなくLayerも管理

#if UNITY_EDITOR
    public void GenerateInEditor()
    {
        // --- 自動生成ギミック用親オブジェクトを用意 ---
        Transform blocksParent = transform.Find("Blocks");
        if (blocksParent == null)
            blocksParent = new GameObject("Blocks").transform;
        blocksParent.SetParent(transform, false);

        // すでにBlocks配下の自動生成ギミックだけ消す
        while (blocksParent.childCount > 0)
        {
            DestroyImmediate(blocksParent.GetChild(0).gameObject);
        }

        // 辞書初期化
        idToPair = new Dictionary<int, GimmickPair>();
        foreach (var pair in gimmickPrefabs)
            idToPair[pair.id] = pair;

        // CSVの空行を除外して読む
        string[] lines = csvFile.text.Split('\n');
        List<string> lineList = new List<string>();
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line.Trim()))
                lineList.Add(line.Trim());
        }

        int rowCount = lineList.Count;
        int colCount = 0;
        foreach (var line in lineList)
        {
            int c = line.Split(',').Length;
            if (c > colCount) colCount = c;
        }

        float cellSize = 1f;

        // Player/Goalは名前で取得（テンプレ内に1つずつ置く運用）
        var playerObj = GameObject.Find("Player");
        var goalObj = GameObject.Find("Goal");

        for (int y = 0; y < rowCount; y++)
        {
            string[] cells = lineList[y].Split(',');
            for (int x = 0; x < cells.Length; x++)
            {
                string cell = cells[x].Trim();
                float px = (x - colCount / 2f + 0.5f) * cellSize;
                float py = -(y - rowCount / 2f + 0.5f) * cellSize;
                Vector3 pos = new Vector3(px, py, 0);

                int id;
                if (int.TryParse(cell, out id))
                {
                    if (id == 1 && playerObj != null)
                    {
                        playerObj.transform.localPosition = pos;
                        continue;
                    }
                    if (id == 2 && goalObj != null)
                    {
                        goalObj.transform.localPosition = pos;
                        continue;
                    }
                }

                // 他ギミックだけBlocks配下に生成
                if (int.TryParse(cell, out id) && idToPair.ContainsKey(id))
                {
                    var pair = idToPair[id];
                    if (pair.prefab != null)
                    {
                        var go = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(pair.prefab, blocksParent);
                        go.transform.localPosition = pos;
                        SetLayerRecursively(go, pair.layer);
                    }
                }
            }
        }

        AdjustCameraToMap_CenterOrigin(colCount, rowCount, cellSize);
    }


    // 子も含めてレイヤーを全部変更
    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    // 中央原点カメラ調整（あなたの既存メソッドでOK）
    void AdjustCameraToMap_CenterOrigin(int colCount, int rowCount, float cellSize = 1f)
    {
        float aspect = 16f / 9f;
        float mapWidth = colCount * cellSize;
        float mapHeight = rowCount * cellSize;
        float orthoSize = Mathf.Max(mapHeight / 2f, mapWidth / 2f / aspect);
        Camera.main.orthographicSize = orthoSize;
        Camera.main.transform.position = new Vector3(0, 0, -10f);
    }
#endif
}
