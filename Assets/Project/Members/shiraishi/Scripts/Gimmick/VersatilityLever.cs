using System.Collections.Generic;
using UnityEngine;

public class VersatilityLever : MonoBehaviour
{
    [Header("起動対象（IActivatable を持つ GameObject または親）")]
    public List<GameObject> targets = new List<GameObject>();

    [Header("起動条件")]
    public KeyCode activateKey = KeyCode.F;
    public bool activateOnTouch = false;

    [Header("レバーの状態表示")]
    public Sprite offSprite;
    public Sprite onSprite;

    [Tooltip("ON/OFF切り替え可能なトグル式レバーか？")]
    public bool isToggle = true;

    [SerializeField] private EffectManager effectManager;

    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider2D;
    private bool isOn = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();// 子から EffectManager を自動で探す（未設定時のみ）
        if (effectManager == null)
        {
            effectManager = GetComponentInChildren<EffectManager>();
            if (effectManager == null)
            {
                Debug.LogWarning($"[DisappearBlock] 子オブジェクトに EffectManager が見つかりません: {gameObject.name}");
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
        // 一方向の場合、すでにONなら無視
        if (!isToggle && isOn) return;

        // 状態更新
        isOn = isToggle ? !isOn : true;
        UpdateSprite();

        // 起動対象の処理実行
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
