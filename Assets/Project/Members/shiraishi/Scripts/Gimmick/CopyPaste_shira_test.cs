using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

// コピー可能オブジェクトの情報を保持するクラス
[System.Serializable]
public class CopyableObjectInfo
{
    public GameObject prefab;   // コピーできるオブジェクト
    public int maxCopies = 1;   // Inspectorで編集
    [HideInInspector] public int currentCopies = 0; // 現在のコピー数、Inspectorでは非表示
}

// コピーがコピー元の情報を持つためのクラス
public class CopyOrigin : MonoBehaviour
{
    public CopyableObjectInfo originInfo;
}

public class CopyPaste_shira_test : MonoBehaviour
{
    [SerializeField, Header("シーン内のコピー可能オブジェクトをここに格納")]
    public List<CopyableObjectInfo> copyableObjects = new List<CopyableObjectInfo>();

    //private List<GameObject> drawIconObjects = new List<GameObject>();
    private Dictionary<GameObject, Image> iconPairs = new Dictionary<GameObject, Image>();

    private GameObject selectedObject;
    private CopyableObjectInfo selectedInfo = null;

    private Vector2 distance_MouseToObject = Vector2.zero;    // 選択時のマウスとオブジェクトの距離

    // アイコン描画用
    public RectTransform canvasRectTransform;
    public GameObject iconPrefab;
    public Sprite normalIconSprite;
    public Sprite limitIconSprite;
    public static CopyPaste_shira_test Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChange;

        // アイコン描画用にリストに追加
        iconPairs.Clear();
        foreach (CopyableObjectInfo info in copyableObjects)
        {
            AddClickHitbox(info.prefab);

            GameObject iconObj = Instantiate(iconPrefab, canvasRectTransform);
            iconObj.SetActive(false);

            Image iconImage = iconObj.GetComponent<Image>();
            if (iconImage == null) return;
            iconPairs.Add(info.prefab, iconImage);
        }

        SetIconVisible(true);
    }

    void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= HandleGameStateChange;
    }

    void Update()
    {
        // ゲームステートが編集モード中なら実行
        if (GameManager.Instance.currentState == GameState.Editing)
        {
            // 左クリックされたオブジェクトを選択する
            if (Input.GetMouseButtonDown(0))
            {
                SelectObject();
            }

            // 選択されたオブジェクトが左クリックされ続けているなら動かせる
            if (Input.GetMouseButton(0))
            {
                DragSelectedObject();
            }

            // 右クリックを押すとコピーする
            if (Input.GetMouseButtonDown(1))
            {
                CopySelectedObject();
            }
        }
    }
    void LateUpdate()
    {
        // ゲームステートが編集モード中なら実行
        if (GameManager.Instance.currentState == GameState.Editing)
        {
            foreach (var pair in iconPairs)
            {
                GameObject obj = pair.Key;
                Image icon = pair.Value;

                if (obj == null)
                {
                    Debug.LogAssertion("コピー可能オブジェクトの配列に空の要素があります");
                    break;
                }
                // オブジェクトの少し上に表示（必要に応じて調整）
                Vector3 offset = new Vector3(-0.5f, 0.5f, 0);
                Vector3 worldPos = obj.transform.position + offset;

                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

                icon.rectTransform.position = screenPos;
            }
        }
    }

    // ゲームステートの変更を検知して処理を実行
    void HandleGameStateChange(GameState prev, GameState curr)
    {
        // 編集モードに切り替わった瞬間にアイコンを表示する
        if (curr == GameState.Editing && prev != GameState.Editing)
        {
            SetIconVisible(true);
        }

        // 編集モードから切り替わったらアイコンを非表示にする
        if (curr != GameState.Editing && prev == GameState.Editing)
        {
            SetIconVisible(false);
        }
    }

    // オブジェクトを選択する
    void SelectObject()
    {
        selectedInfo = null;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int clickLayer = LayerMask.GetMask("ClickDetection");

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, clickLayer);

        if (hit.collider != null)
        {
            GameObject hitObj = hit.collider.gameObject;
            GameObject rootObj = hitObj.transform.root.gameObject;

            selectedInfo = copyableObjects.Find(x => x.prefab == rootObj);
            if (selectedInfo == null)
            {
                var origin = rootObj.GetComponent<CopyOrigin>();
                if (origin != null)
                {
                    selectedObject = rootObj;
                    selectedInfo = origin.originInfo;
                    distance_MouseToObject = (Vector2)selectedObject.transform.position - mousePos;
                    Debug.Log($"{rootObj.name} を選択 元オブジェクト {selectedInfo.prefab.name}");
                }
                else
                {
                    Debug.Log("このオブジェクトはコピー・選択対象外です");
                }
            }
            else
            {
                selectedObject = rootObj;
                distance_MouseToObject = (Vector2)selectedObject.transform.position - mousePos;
                Debug.Log($"{rootObj.name} を選択");
            }
        }
        else
        {
            selectedInfo = null;
        }
    }


    // オブジェクトをコピー
    public bool CopySelectedObject()
    {
        if (selectedInfo == null)
        {
            Debug.Log("何も選択されていません");
            return false;
        }
        if (selectedInfo.currentCopies >= selectedInfo.maxCopies)
        {
            Debug.Log("コピー上限に達しています");
            return false;
        }

        // マウス座標にコピーを生成し、コピー数カウントを増加、名前を変更
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var clone = Instantiate(selectedInfo.prefab, mousePos, selectedInfo.prefab.transform.rotation);
        selectedInfo.currentCopies++;
        clone.name = $"{selectedInfo.prefab.name}_copy ({selectedInfo.currentCopies})";

        // コピー元の情報を持たせる
        var origin = clone.AddComponent<CopyOrigin>();
        origin.originInfo = selectedInfo;


        GameObject iconObj = Instantiate(iconPrefab, canvasRectTransform);
        iconObj.SetActive(true);

        Image iconImage = iconObj.GetComponent<Image>();
        iconPairs.Add(clone, iconImage);

        UpdateAllIconSprites();

        Debug.Log($"{clone.name} を生成");
        return true;
    }

    // 選択されたオブジェクトを移動させる
    void DragSelectedObject()
    {
        if (selectedInfo != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedObject.transform.SetPositionAndRotation(mousePos + distance_MouseToObject, transform.rotation);
        }
    }

    // 選択できるオブジェクトにアイコンを表示する
    void SetIconVisible(bool visible)
    {
        foreach (var icon in iconPairs.Values)
        {
            icon.gameObject.SetActive(visible);
        }
    }

    // コピー上限に達しているなら画像を変更
    public void UpdateAllIconSprites()
    {
        foreach (var pair in iconPairs)
        {
            GameObject obj = pair.Key;
            Image icon = pair.Value;

            var info = GetCopyableInfo(obj);

            if (info != null && info.currentCopies >= info.maxCopies)
            {
                icon.sprite = limitIconSprite;
            }
            else
            {
                icon.sprite = normalIconSprite;
            }
        }
    }

    private CopyableObjectInfo GetCopyableInfo(GameObject obj)
    {
        var origin = obj.GetComponent<CopyOrigin>();
        if (origin != null)
        {
            return origin.originInfo;
        }

        // シーン上のオブジェクトを参照で一致させる
        return copyableObjects.FirstOrDefault(x => x.prefab == obj);
    }

    void AddClickHitbox(GameObject obj)
    {
        if (obj.transform.Find("ClickHitbox") != null)
            return;

        GameObject hitbox = new GameObject("ClickHitbox");
        hitbox.transform.SetParent(obj.transform);
        hitbox.transform.localPosition = Vector3.zero;
        hitbox.transform.localScale = Vector3.one;

        BoxCollider2D col = hitbox.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        // 子にあるSpriteRendererも含めて探す
        SpriteRenderer spriteRenderer = obj.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            Vector2 pixelSize = spriteRenderer.sprite.rect.size;
            float ppu = spriteRenderer.sprite.pixelsPerUnit;
            Vector2 localSize = pixelSize / ppu;

            float minSize = 0.5f;
            localSize.x = Mathf.Max(localSize.x, minSize);
            localSize.y = Mathf.Max(localSize.y, minSize);

            col.size = localSize;
        }
        else
        {
            Debug.LogWarning($"{obj.name} に有効な SpriteRenderer が見つかりませんでした");
        }

        hitbox.layer = LayerMask.NameToLayer("ClickDetection");
    }
}