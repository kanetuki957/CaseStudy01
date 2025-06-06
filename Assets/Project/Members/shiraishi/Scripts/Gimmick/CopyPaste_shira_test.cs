using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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


    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChange;

        // アイコン描画用にリストに追加
        iconPairs.Clear();
        foreach (CopyableObjectInfo info in copyableObjects)
        {
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
        foreach (var pair in iconPairs)
        {
            GameObject obj = pair.Key;
            Image icon = pair.Value;

            // オブジェクトの少し上に表示（必要に応じて調整）
            Vector3 offset = new Vector3(-0.5f, 0.5f, 0);
            Vector3 worldPos = obj.transform.position + offset;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            icon.rectTransform.position = screenPos;
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
        if(curr != GameState.Editing && prev == GameState.Editing)
        {
            SetIconVisible(false);
        }
    }

    // オブジェクトを選択する
    void SelectObject()
    {
        selectedInfo = null;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            selectedInfo = copyableObjects.Find(x => x.prefab == hit.collider.gameObject);
            if (selectedInfo == null)
            {
                // リストに登録されていないオブジェクトを選択した場合
                var origin = hit.collider.gameObject.GetComponent<CopyOrigin>();
                if (origin != null)
                {
                    // リストに直接登録されていないが、コピー元オブジェクトが登録されていた場合
                    selectedObject = hit.collider.gameObject;
                    selectedInfo = origin.originInfo;
                    distance_MouseToObject = (Vector2)selectedObject.transform.position - mousePos;
                    Debug.Log($"{hit.collider.gameObject.name} を選択 元オブジェクト {selectedInfo.prefab.name}");
                }
                else
                {
                    Debug.Log("このオブジェクトはコピー・選択対象外です");
                }
            }
            else
            {
                // リストに登録されているオブジェクトを選択した場合
                selectedObject = hit.collider.gameObject;
                distance_MouseToObject = (Vector2)selectedObject.transform.position - mousePos;
                Debug.Log($"{hit.collider.gameObject.name} を選択");
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

    public void UpdateAllIconSprites()
    {
        foreach (var pair in iconPairs)
        {
            GameObject obj = pair.Key;
            Image icon = pair.Value;

            // CopyOriginがついていればコピー済み、なければ元Prefab
            CopyOrigin origin = obj.GetComponent<CopyOrigin>();
            CopyableObjectInfo info = null;

            if (origin != null)
            {
                info = origin.originInfo;
            }
            else
            {
                // 元Prefabの場合
                info = copyableObjects.Find(x => x.prefab == obj);
            }

            // 上限判定＆Sprite切り替え
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

}
