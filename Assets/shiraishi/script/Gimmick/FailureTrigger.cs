// �t�@�C����: FailureTrigger.cs

using UnityEngine;
using UnityEngine.SceneManagement;

public class FailureTrigger : MonoBehaviour
{
    [Tooltip("�Q�[���I�[�o�[���ɑJ�ڂ���V�[���i���w��̏ꍇ�͌��݂̃V�[�����ēǂݍ��݁j")]
    public string gameOverScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[FailureTrigger] �v���C���[���g���K�[�ɐG��܂���");

            if (!string.IsNullOrEmpty(gameOverScene))
            {
                SceneManager.LoadScene(gameOverScene);
            }
            else
            {
                // �V�[���������[�h�i���g���C�p�j
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
