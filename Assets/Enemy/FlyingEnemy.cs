using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float moveSpeed = 1f;       // ¶•ûŒü‚Ö‚ÌˆÚ“®‘¬“x
    public float floatSpeed = 2f;      // ã‰º‚Ì—h‚ê‚Ì‘¬‚³
    public float floatHeight = 2f;     // ã‰º‚ÌU‚ê•

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // ¶‚É­‚µ‚¸‚ÂˆÚ“®
        float moveX = transform.position.x - moveSpeed * Time.deltaTime;

        // ã‰º‚É‚ä‚ç‚ä‚ç
        float moveY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // V‚µ‚¢ˆÊ’u‚ÉˆÚ“®
        transform.position = new Vector3(moveX, moveY, transform.position.z);
    }
}
