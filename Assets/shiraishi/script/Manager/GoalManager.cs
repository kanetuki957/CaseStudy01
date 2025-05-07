using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    // ���łɃG���[��\���ς݂��ǂ����̃t���O�i�����G���[�����x���o���Ȃ����߁j
    private bool hasShownError = false;
    private bool hasShownUnregisteredPlayerWarning = false;

    private bool wasKeyPresentAtStart = false;

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

        // �V�[����� "key" ���܂ރA�C�e�����擾
        var keysInScene = GameObject.FindGameObjectsWithTag("Item")
            .Where(item => item.name.StartsWith("key"))
            .ToArray();

        wasKeyPresentAtStart = keysInScene.Length > 0;

        // �v���C���[�ƃS�[������goalPairs����擾
        int playerCount = goalPairs.Count;
        int keyCount = keysInScene.Length;
        int goalCount = goalPairs.Select(p => p.goal).Distinct().Count();

        // ������v���Ȃ��ꍇ�Ɍx���i��������Ƃ������`�F�b�N�j
        if (wasKeyPresentAtStart && (keyCount != playerCount || goalCount != playerCount))
        {
            Debug.LogWarning(
                $"GoalManager: ��({keyCount})�E�v���C���[({playerCount})�E�S�[��({goalCount}) �̐�����v���Ă��܂���I�Ӑ}�����d�l�ł��邩�m�F���Ă��������B"
            );
        }
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

        if (AllPlayersOnGoals() && AllPlayersHaveKey())
        {
            SceneManager.LoadScene(next_scene);
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

}
