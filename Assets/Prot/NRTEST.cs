using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NRTEST : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    // 橋の状態切り替えを行うオブジェクトが当たったかを確認するスクリプト
    [SerializeField] private NRTEST02 hitTrigger;

    // コピペ最大数を入力
    [SerializeField] private int maxCopySize = 10;

    // 変換前オブジェクトを設定
    [SerializeField] private GameObject beforeObject;
    // 変換後オブジェクトを設定
    [SerializeField] private GameObject changedObject;


    // 生成オブジェクト(変換前)を保存
    // こいつに変化前の橋を入れる
    // セットした橋はBridgeManagerの子オブジェクトにする
    [SerializeField] private List<GameObject> beforeObjectList = new List<GameObject>();
    // 生成オブジェクト(変換後)を保存
    List<GameObject> changedObjectList = new List<GameObject>();


    // 選択しているオブジェクトを保存
    private GameObject selectObject;



    private void Start()
    {
        // 事前にオブジェクトを生成
        for(int i = 0; i< maxCopySize; i++)
        {
            if (beforeObjectList.Count < maxCopySize)
            {
                // 変化前オブジェクトを作成
                GameObject newBeforeObj = Instantiate(beforeObject, transform);
                newBeforeObj.SetActive(false);
                beforeObjectList.Add(newBeforeObj);
            }

            // 変化後オブジェクトを作成
            GameObject newChangedObj = Instantiate(changedObject, transform);
            newChangedObj.SetActive(false);
            changedObjectList.Add(newChangedObj);
        }

        //  一つ目のオブジェクトを表示しておく
        beforeObjectList[0].SetActive(true);
    }


    private void Update()
    {
        Copy();
        Paste();

        // ハートに当たったら
        if(hitTrigger.isHit)
        {
            for(int i = 0; i < beforeObjectList.Count; i++)
            {
                // 変化前オブジェクト達でアクティブなものを探す
                if (beforeObjectList[i].activeInHierarchy)
                {
                    beforeObjectList[i].SetActive(false);

                    // 変化後オブジェクトをアクティブに
                    changedObjectList[i].SetActive(true);
                    // 座標を設定
                    changedObjectList[i].transform.position = beforeObjectList[i].transform.position;

                }
            }

            // ここで false に再設定をしないと永遠に反応しちゃう
            hitTrigger.ResetHit();
        }
    }



    void Copy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = GetCameraUnderMouse();
            if (cam == null) return;

            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                selectObject = hit.collider.gameObject;
                Debug.Log("Selected: " + selectObject.name);
            }
        }
    }


    void Paste()
    {
        if (selectObject.CompareTag("Finish") && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
        {
            Camera cam = GetCameraUnderMouse();
            if (cam == null) return;

            // 変更したかを保存
            for (int i = 0; i < beforeObjectList.Count; i++)
            {
                // 非アクティブのオブジェクトを見つけたら
                if (!beforeObjectList[i].activeInHierarchy)
                {
                    // アクティブに変更
                    beforeObjectList[i].SetActive(true);
                    Vector2 spawnPos = cam.ScreenToWorldPoint(Input.mousePosition);

                    beforeObjectList[i].transform.position = spawnPos;
                    // 変更したら終了
                    break;
                }
            }
        }
    }



    Camera GetCameraUnderMouse()
    {
        if (IsMouseInCamera(camera1)) return camera1;
        if (IsMouseInCamera(camera2)) return camera2;
        return null;
    }

    bool IsMouseInCamera(Camera cam)
    {
        Vector3 viewPortPos = cam.ScreenToViewportPoint(Input.mousePosition);
        return viewPortPos.x >= 0 && viewPortPos.x <= 1 && viewPortPos.y >= 0 && viewPortPos.y <= 1;
    }



}
