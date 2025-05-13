using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlock : MonoBehaviour
{
    GameObject copiedBlock;
    GameObject movingBlock;
    public int maxcopy;
    private int copynumber;
    public int maxpaste;
    private int pastenumber;
    public float speed = 20f;

    private void Start()
    {
        copynumber = 0;
        pastenumber = 0;
    }




    void Update()
    {
        if (maxcopy > copynumber && Input.GetKeyDown(KeyCode.C))
        {
            CopyBlock();
        }
        if (maxpaste > pastenumber && Input.GetKeyDown(KeyCode.V))
        {
            PasteBlock();
        }
        moveBlock();
    }


    void CopyBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.GetComponent<BoxCollider>() != null)
            {
                copiedBlock = hit.collider.gameObject;
                Debug.Log("ブロックをコピーしました！");
                copynumber++;
            }
        }
    }

    void PasteBlock()
    {
        if (copiedBlock != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 pastePosition = Vector3.zero;
            Vector3 blockSize = copiedBlock.GetComponent<Renderer>().bounds.size; // ブロックサイズを取得

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                pastePosition = hit.point;
            }
            else
            {
                pastePosition = Camera.main.transform.position + Camera.main.transform.forward * 3f;
            }

            // **衝突判定（OverlapBoxを使用）**
            if (Physics.OverlapBox(pastePosition, blockSize / 2.0f).Length == 0)
            {
                Instantiate(copiedBlock, pastePosition, copiedBlock.transform.rotation);
                Debug.Log("ブロックをペーストしました！");
                pastenumber++;
            }
            else
            {
                Debug.Log("ペーストできません！既にブロックがあります。");
            }
        }
    }  
    void moveBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.GetComponent<CreateBlock>() != null)
            {
                movingBlock = hit.collider.gameObject;
              
            }
            
        }
        if (Input.GetMouseButton(1)) // 左クリックが押され続けているかチェック
        {
            Vector3 mousePos = Input.mousePosition; // マウスの画面上の位置を取得
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f)); // ワールド座標に変換

            // 他のブロックとの重なりを判定
            Collider2D hit1 = Physics2D.OverlapBox(mousePos, transform.localScale, 0);
            if (hit1 == null) // 何もない場所なら移動
            {
                // オブジェクトをマウスの位置に移動（スムーズにするためにLerp使用）
                movingBlock.transform.position = Vector3.Lerp(movingBlock.transform.position, mousePos, speed * Time.deltaTime);

            }

        }

    }
    public void PlusCopy(int copy)
    {
        maxcopy = maxcopy + copy;
        Debug.Log(maxcopy);
    }
    public void MinusCopy(int copy)
    {
        maxcopy = maxcopy - copy;
        Debug.Log(maxcopy);
    }


}