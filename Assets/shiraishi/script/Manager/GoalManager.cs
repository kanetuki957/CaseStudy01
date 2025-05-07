using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    // すでにエラーを表示済みかどうかのフラグ（同じエラーを何度も出さないため）
    private bool hasShownError = false;
    private bool hasShownUnregisteredPlayerWarning = false;

    private bool wasKeyPresentAtStart = false;

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

        // シーン上の "key" を含むアイテムを取得
        var keysInScene = GameObject.FindGameObjectsWithTag("Item")
            .Where(item => item.name.StartsWith("key"))
            .ToArray();

        wasKeyPresentAtStart = keysInScene.Length > 0;

        // プレイヤーとゴール数をgoalPairsから取得
        int playerCount = goalPairs.Count;
        int keyCount = keysInScene.Length;
        int goalCount = goalPairs.Select(p => p.goal).Distinct().Count();

        // 数が一致しない場合に警告（鍵があるときだけチェック）
        if (wasKeyPresentAtStart && (keyCount != playerCount || goalCount != playerCount))
        {
            Debug.LogWarning(
                $"GoalManager: 鍵({keyCount})・プレイヤー({playerCount})・ゴール({goalCount}) の数が一致していません！意図した仕様であるか確認してください。"
            );
        }
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

        if (AllPlayersOnGoals() && AllPlayersHaveKey())
        {
            SceneManager.LoadScene(next_scene);
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
        if (!wasKeyPresentAtStart)
        {
            return true;
        }

        foreach (var pair in goalPairs)
        {
            if (pair.player == null)
            {
                return false;
            }

            var itemComponent = pair.player.GetComponent<PlayerInventory>();
            if (itemComponent == null || itemComponent.collectedItems == null)
            {
                return false;
            }

            if (!itemComponent.collectedItems.Contains("key"))
            {
                return false;
            }
        }

        return true;
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

}
