using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    // すでにエラーを表示済みかどうかのフラグ（同じエラーを何度も出さないため）
    private bool hasShownError = false;
    private bool hasShownUnregisteredPlayerWarning = false;

    // プレイヤーと対応するゴールをセットで管理する構造体
    [System.Serializable]
    public class GoalPair
    {
        public GameObject player; // ゴール判定対象のプレイヤー
        public Goal goal;         // 対応するゴール（Goal.csがアタッチされていること）
    }

    // インスペクターから設定できる、プレイヤーとゴールの対応リスト
    public List<GoalPair> goalPairs = new List<GoalPair>();

    // クリア時に遷移するシーン名（Build Settings に登録されている必要あり）
    public string next_scene;

    void Start()
    {
        CheckUnregisteredPlayers(); // ゲーム開始時に未登録プレイヤーを警告
        

    }

    void Update()
    {
        if (goalPairs == null || goalPairs.Count == 0)
        {
            if (!hasShownError)
            {
                Debug.LogError("GoalManager: ゴールのペアリストが空です。設定を確認してください。");
                hasShownError = true;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var pair in goalPairs)
            {
                var items = pair.player.GetComponent<aitem>()?.collectedItems;
                Debug.Log($"[L CHECK] {pair.player.name} のアイテム: {string.Join(", ", items ?? new List<string>())}");
            }
        }


        if (AllPlayersOnGoals())
        {
            Debug.Log("[CHECK] 全プレイヤーがゴールに触れている");
            if (AllPlayersHaveKey())
            {
                Debug.Log("[CHECK] 全プレイヤーがキーを持っている → シーン遷移実行");
                //SceneManager.LoadScene(next_scene);
            }
            else
            {
                Debug.Log("[CHECK] キーを持っていないプレイヤーがいる → 遷移しない");
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            DebugTouchingPlayers();
            foreach (var pair in goalPairs)
            {
                Debug.Log($"Registered Player: {pair.player.name}, ID: {pair.player.GetInstanceID()}");
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var pair in goalPairs)
            {
                var itemComponent = pair.player.GetComponent<aitem>();
                if (itemComponent != null)
                {
                    string playerName = pair.player.name;
                    string itemList = string.Join(", ", itemComponent.collectedItems);
                    Debug.Log($"[L CHECK] {playerName} のアイテム: {itemList}");
                }
                else
                {
                    Debug.LogWarning($"[L CHECK] {pair.player.name} に aitem がついてない！");
                }
            }
        }

    }


    // 全プレイヤーが対応するゴールに触れているかをチェック
    private bool AllPlayersOnGoals()
    {
        foreach (var pair in goalPairs)
        {
            if (!pair.goal.IsPlayerTouching(pair.player))
                return false;
        }
        return true;
    }

    // キーが必要かどうかを判断し、所持状況をチェック
    private bool AllPlayersHaveKey()
    {
        if (GameObject.Find("key") == null)
        {
            Debug.LogWarning("GoalManager: 'key' オブジェクトがシーン上に存在しないため、キー取得判定をスキップします。");
            return true;
        }

        bool allHaveKey = true;

        foreach (var pair in goalPairs)
        {
            if (pair.player == null)
            {
                Debug.LogError("[KEY CHECK] goalPairs に null の player が含まれています！");
                allHaveKey = false;
                continue;
            }

            string name = pair.player.name;
            int id = pair.player.GetInstanceID();
            var itemComponent = pair.player.GetComponent<aitem>();

            if (itemComponent == null)
            {
                Debug.LogWarning($"[KEY CHECK] {name} (ID: {id}) に aitem がアタッチされていません！");
                allHaveKey = false;
                continue;
            }

            var items = itemComponent.collectedItems;
            if (items == null)
            {
                Debug.LogWarning($"[KEY CHECK] {name} (ID: {id}) の collectedItems が null です！");
                allHaveKey = false;
                continue;
            }

            Debug.Log($"[KEY CHECK] {name} (ID: {id}) のアイテム: {string.Join(", ", items)}");

            if (!items.Contains("key"))
            {
                Debug.LogWarning($"[KEY CHECK] {name} (ID: {id}) は 'key' を持っていません！");
                allHaveKey = false;
            }
            else
            {
                Debug.Log($"[KEY CHECK] {name} (ID: {id}) は 'key' を持っています！");
            }
        }

        if (allHaveKey)
        {
            Debug.Log("[CHECK] 全プレイヤーがキーを持っている → シーン遷移実行");
        }
        else
        {
            Debug.Log("[CHECK] キーを持っていないプレイヤーがいる → 遷移しない");
        }

        return allHaveKey;
    }





    // シーン上のプレイヤーが goalPairs に登録されているかを検証
    private void CheckUnregisteredPlayers()
    {
        GameObject[] allPlayersInScene = GameObject.FindGameObjectsWithTag("Player");

        HashSet<GameObject> registeredPlayers = new HashSet<GameObject>();
        foreach (var pair in goalPairs)
        {
            if (pair.player != null)
                registeredPlayers.Add(pair.player);
        }

        foreach (var player in allPlayersInScene)
        {
            if (!registeredPlayers.Contains(player))
            {
                if (!hasShownUnregisteredPlayerWarning)
                {
                    Debug.LogWarning($"GoalManager: シーン内のプレイヤー '{player.name}' が goalPairs に登録されていません！");
                    hasShownUnregisteredPlayerWarning = true;
                }
            }
        }
    }

    // 各プレイヤーがゴールに触れているかをコンソールに出力（デバッグ用）
    void DebugTouchingPlayers()
    {
        foreach (var pair in goalPairs)
        {
            bool touching = pair.goal.IsPlayerTouching(pair.player);
            Debug.Log($"[DEBUG] {pair.player.name} は {(touching ? "ゴールに触れている" : "触れていない")}");
        }
    }
    void DebugKeyCheck()
    {
        foreach (var pair in goalPairs)
        {
            var itemComponent = pair.player.GetComponent<aitem>();
            string playerName = pair.player.name;

            if (itemComponent == null)
            {
                Debug.LogWarning($"[DEBUG] {playerName} に aitem コンポーネントがありません！");
            }
            else
            {
                bool hasKey = itemComponent.collectedItems.Contains("key");
                Debug.Log($"[DEBUG] {playerName} は 'key' を {(hasKey ? "持っている" : "持っていない")}");
            }
        }
    }


}
