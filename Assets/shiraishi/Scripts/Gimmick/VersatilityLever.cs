using System.Collections.Generic;
using UnityEngine;

public class VersatilityLever : MonoBehaviour
{
    [Header("�N���ΏہiIActivatable ������ GameObject �܂��͐e�j")]
    public List<GameObject> targets = new List<GameObject>();

    [Header("�N������")]
    public KeyCode activateKey = KeyCode.F;
    public bool activateOnTouch = false;

    [Header("���o�[�̏�ԕ\��")]
    public Sprite offSprite;
    public Sprite onSprite;

    [Tooltip("ON/OFF�؂�ւ��\�ȃg�O�������o�[���H")]
    public bool isToggle = true;

    [SerializeField] private EffectManager effectManager;

    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider2D;
    private bool isOn = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();// �q���� EffectManager �������ŒT���i���ݒ莞�̂݁j
        if (effectManager == null)
        {
            effectManager = GetComponentInChildren<EffectManager>();
            if (effectManager == null)
            {
                Debug.LogWarning($"[DisappearBlock] �q�I�u�W�F�N�g�� EffectManager ��������܂���: {gameObject.name}");
            }
        }
        UpdateSprite();
    }

    void Update()
    {
        if (!activateOnTouch && playerCollider2D != null && Input.GetKeyDown(activateKey))
        {
            ActivateIfNeeded();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollider2D = other;

            if (activateOnTouch)
            {
                ActivateIfNeeded();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollider2D = null;
        }
    }

    void ActivateIfNeeded()
    {
        // ������̏ꍇ�A���ł�ON�Ȃ疳��
        if (!isToggle && isOn) return;

        // ��ԍX�V
        isOn = isToggle ? !isOn : true;
        UpdateSprite();

        // �N���Ώۂ̏������s
        foreach (var targetObj in targets)
        {
            if (targetObj == null) continue;

            var direct = targetObj.GetComponents<IActivatable>();
            foreach (var a in direct) a.Activate();

            var children = targetObj.GetComponentsInChildren<IActivatable>(true);
            foreach (var a in children)
            {
                if (System.Array.IndexOf(direct, a) == -1)
                    a.Activate();
            }
        }
    }

    void UpdateSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isOn ? onSprite : offSprite;
        }
    }
}
