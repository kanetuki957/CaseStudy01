using System.Collections.Generic;
using System.Linq;
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
                //Debug.LogWarning($"[DisappearBlock] 子オブジェクトに EffectManager が見つかりません: {gameObject.name}");
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

        foreach (var targetObj in targets)
        {
            if (targetObj == null) continue;

            // 対象そのものを処理
            ActivateAllIActivatablesIn(targetObj);

            // CopyableObjectInfo を元に、同じ originInfo を持つクローンも処理
            var origin = targetObj.GetComponent<CopyOrigin>();
            CopyableObjectInfo originInfo = null;

            if (origin != null)
            {
                originInfo = origin.originInfo;
            }
            else
            {
                // プレハブそのものだった場合も含めて探す
                originInfo = CopyPaste_shira_test.Instance.copyableObjects?.Find(x => x.prefab == targetObj);
            }

            if (originInfo != null)
            {
                // シーン上のクローン（コピー）すべてを検索して起動
                var clones = FindObjectsOfType<CopyOrigin>()
                             .Where(o => o.originInfo == originInfo)
                             .Select(o => o.gameObject);

                foreach (var clone in clones)
                {
                    ActivateAllIActivatablesIn(clone);
                }
            }
        }
    }

    // ヘルパーメソッド：対象オブジェクトにあるすべての IActivatable を実行
    void ActivateAllIActivatablesIn(GameObject obj)
    {
        var list1 = obj.GetComponents<IActivatable>();
        var list2 = obj.GetComponentsInChildren<IActivatable>(true);

        foreach (var a in list1.Concat(list2).Distinct())
        {
            a.Activate();
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
