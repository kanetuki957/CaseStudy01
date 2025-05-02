using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    // ���łɃG���[��\���ς݂��ǂ����̃t���O�i�����G���[�����x���o���Ȃ����߁j
    private bool hasShownError = false;
    private bool hasShownUnregisteredPlayerWarning = false;

    // �v���C���[�ƑΉ�����S�[�����Z�b�g�ŊǗ�����\����
    [System.Serializable]
    public class GoalPair
    {
        public GameObject player; // �S�[������Ώۂ̃v���C���[
        public Goal goal;         // �Ή�����S�[���iGoal.cs���A�^�b�`����Ă��邱�Ɓj
    }

    // �C���X�y�N�^�[����ݒ�ł���A�v���C���[�ƃS�[���̑Ή����X�g
    public List<GoalPair> goalPairs = new List<GoalPair>();

    // �N���A���ɑJ�ڂ���V�[�����iBuild Settings �ɓo�^����Ă���K�v����j
    public string next_scene;

    void Start()
    {
        CheckUnregisteredPlayers(); // �Q�[���J�n���ɖ��o�^�v���C���[���x��
        

    }

    void Update()
    {
        if (goalPairs == null || goalPairs.Count == 0)
        {
            if (!hasShownError)
            {
                Debug.LogError("GoalManager: �S�[���̃y�A���X�g����ł��B�ݒ���m�F���Ă��������B");
                hasShownError = true;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var pair in goalPairs)
            {
                var items = pair.player.GetComponent<aitem>()?.collectedItems;
                Debug.Log($"[L CHECK] {pair.player.name} �̃A�C�e��: {string.Join(", ", items ?? new List<string>())}");
            }
        }


        if (AllPlayersOnGoals())
        {
            Debug.Log("[CHECK] �S�v���C���[���S�[���ɐG��Ă���");
            if (AllPlayersHaveKey())
            {
                Debug.Log("[CHECK] �S�v���C���[���L�[�������Ă��� �� �V�[���J�ڎ��s");
                //SceneManager.LoadScene(next_scene);
            }
            else
            {
                Debug.Log("[CHECK] �L�[�������Ă��Ȃ��v���C���[������ �� �J�ڂ��Ȃ�");
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
                    Debug.Log($"[L CHECK] {playerName} �̃A�C�e��: {itemList}");
                }
                else
                {
                    Debug.LogWarning($"[L CHECK] {pair.player.name} �� aitem �����ĂȂ��I");
                }
            }
        }

    }


    // �S�v���C���[���Ή�����S�[���ɐG��Ă��邩���`�F�b�N
    private bool AllPlayersOnGoals()
    {
        foreach (var pair in goalPairs)
        {
            if (!pair.goal.IsPlayerTouching(pair.player))
                return false;
        }
        return true;
    }

    // �L�[���K�v���ǂ����𔻒f���A�����󋵂��`�F�b�N
    private bool AllPlayersHaveKey()
    {
        if (GameObject.Find("key") == null)
        {
            Debug.LogWarning("GoalManager: 'key' �I�u�W�F�N�g���V�[����ɑ��݂��Ȃ����߁A�L�[�擾������X�L�b�v���܂��B");
            return true;
        }

        bool allHaveKey = true;

        foreach (var pair in goalPairs)
        {
            if (pair.player == null)
            {
                Debug.LogError("[KEY CHECK] goalPairs �� null �� player ���܂܂�Ă��܂��I");
                allHaveKey = false;
                continue;
            }

            string name = pair.player.name;
            int id = pair.player.GetInstanceID();
            var itemComponent = pair.player.GetComponent<aitem>();

            if (itemComponent == null)
            {
                Debug.LogWarning($"[KEY CHECK] {name} (ID: {id}) �� aitem ���A�^�b�`����Ă��܂���I");
                allHaveKey = false;
                continue;
            }

            var items = itemComponent.collectedItems;
            if (items == null)
            {
                Debug.LogWarning($"[KEY CHECK] {name} (ID: {id}) �� collectedItems �� null �ł��I");
                allHaveKey = false;
                continue;
            }

            Debug.Log($"[KEY CHECK] {name} (ID: {id}) �̃A�C�e��: {string.Join(", ", items)}");

            if (!items.Contains("key"))
            {
                Debug.LogWarning($"[KEY CHECK] {name} (ID: {id}) �� 'key' �������Ă��܂���I");
                allHaveKey = false;
            }
            else
            {
                Debug.Log($"[KEY CHECK] {name} (ID: {id}) �� 'key' �������Ă��܂��I");
            }
        }

        if (allHaveKey)
        {
            Debug.Log("[CHECK] �S�v���C���[���L�[�������Ă��� �� �V�[���J�ڎ��s");
        }
        else
        {
            Debug.Log("[CHECK] �L�[�������Ă��Ȃ��v���C���[������ �� �J�ڂ��Ȃ�");
        }

        return allHaveKey;
    }





    // �V�[����̃v���C���[�� goalPairs �ɓo�^����Ă��邩������
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
                    Debug.LogWarning($"GoalManager: �V�[�����̃v���C���[ '{player.name}' �� goalPairs �ɓo�^����Ă��܂���I");
                    hasShownUnregisteredPlayerWarning = true;
                }
            }
        }
    }

    // �e�v���C���[���S�[���ɐG��Ă��邩���R���\�[���ɏo�́i�f�o�b�O�p�j
    void DebugTouchingPlayers()
    {
        foreach (var pair in goalPairs)
        {
            bool touching = pair.goal.IsPlayerTouching(pair.player);
            Debug.Log($"[DEBUG] {pair.player.name} �� {(touching ? "�S�[���ɐG��Ă���" : "�G��Ă��Ȃ�")}");
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
                Debug.LogWarning($"[DEBUG] {playerName} �� aitem �R���|�[�l���g������܂���I");
            }
            else
            {
                bool hasKey = itemComponent.collectedItems.Contains("key");
                Debug.Log($"[DEBUG] {playerName} �� 'key' �� {(hasKey ? "�����Ă���" : "�����Ă��Ȃ�")}");
            }
        }
    }


}
