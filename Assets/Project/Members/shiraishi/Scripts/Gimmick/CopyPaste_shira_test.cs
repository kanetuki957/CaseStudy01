using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    private GameObject selectedObject;

    [SerializeField, Header("シーン内のコピー可能オブジェクトをここに格納")]
    public List<CopyableObjectInfo> copyableObjects = new List<CopyableObjectInfo>();
    private CopyableObjectInfo selectedInfo = null;

    void Update()
    {
        // ゲームステートが編集モード中なら実行
        if (GameManager.Instance.CurrentState == GameState.Editing)
        {
            // 左クリックされたオブジェクトを選択する
            if (Input.GetMouseButtonDown(0))
            {
                SelectObject();
            }

            // 選択されたオブジェクトが左クリックされ続けているなら動かせる
            if (Input.GetMouseButton(0))
            {
                DragObject();
            }

            // 右クリックを押すとコピーする
            if (Input.GetMouseButtonDown(1))
            {
                CopySelectedObject();
            }

        }
    }

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
                // 直接なければ、CopyOriginを探す
                var origin = hit.collider.gameObject.GetComponent<CopyOrigin>();
                if (origin != null)
                {
                    selectedObject = hit.collider.gameObject; // 現在のままだとリスト外のオブジェクトも移動できてしまう
                    selectedInfo = origin.originInfo;
                    Debug.Log($"{hit.collider.gameObject.name} の元オブジェクト {selectedInfo.prefab.name} を参照して選択");
                }
                else
                {
                    Debug.Log("このオブジェクトはコピー・選択対象外です");
                }
            }
            else
            {
                selectedObject = hit.collider.gameObject;
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


        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var clone = Instantiate(selectedInfo.prefab, mousePos, selectedInfo.prefab.transform.rotation);
        selectedInfo.currentCopies++;
        clone.name = $"{selectedInfo.prefab.name}_copy ({selectedInfo.currentCopies})";

        var origin = clone.AddComponent<CopyOrigin>();
        origin.originInfo = selectedInfo;

        Debug.Log($"{clone.name} を生成");
        return true;
    }


    void DragObject()
    {
        if (selectedInfo != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedObject.transform.SetPositionAndRotation(mousePos, transform.rotation);
        }
    }

}
