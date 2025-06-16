using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CopyPaste スクリプトの動作を制限：
/// ・指定された GameObject のみコピー許可
/// ・コピー回数は最大2回まで
/// ・貼り付けは dropTarget の位置に行う
/// </summary>
public class CopyPasteLimiter : MonoBehaviour
{
    public GameObject allowedObject;   // コピー許可対象（1つだけ）
    public GameObject dropTarget;      // 複製を出現させる位置
    private int copyCount = 0;

    private CopyPaste copyPasteRef;

    void Start()
    {
        copyPasteRef = GetComponent<CopyPaste>();
    }

    void Update()
    {
        RestrictCopyPaste();
    }

    void RestrictCopyPaste()
    {
        if (copyPasteRef == null || allowedObject == null || dropTarget == null) return;

        // 選択されたオブジェクトがコピー対象と違うなら、貼り付け封じる
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
        {
            GameObject selected = GetSelectedObject();
            if (selected != allowedObject || copyCount >= 2)
            {
                // 貼り付けさせないように一時的にコピー対象を null にする
                SetCopiedObject(null);
                return;
            }

            // コピー処理：指定位置に貼り付け
            GameObject newObj = Instantiate(allowedObject, dropTarget.transform.position, allowedObject.transform.rotation);
            newObj.name = allowedObject.name + "_Copy" + (copyCount + 1);
            copyCount++;

            // CopyPaste の選択状態をリセット
            SetSelectedObject(null);
        }
    }

    // CopyPaste の selectedObject にアクセス
    GameObject GetSelectedObject()
    {
        var field = typeof(CopyPaste).GetField("selectedObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return field?.GetValue(copyPasteRef) as GameObject;
    }

    // CopyPaste の selectedObject を変更（貼り付け禁止や解除）
    void SetSelectedObject(GameObject obj)
    {
        var field = typeof(CopyPaste).GetField("selectedObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            field.SetValue(copyPasteRef, obj);
    }

    // CopyPaste の copiedObject を制御（null にすれば貼り付けできなくなる）
    void SetCopiedObject(GameObject obj)
    {
        var field = typeof(CopyPaste).GetField("copiedObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            field.SetValue(copyPasteRef, obj);
    }
}