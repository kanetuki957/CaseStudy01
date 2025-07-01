using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    [Header("à⁄ìÆÇÃê›íË")]
    public float moveDistance = 2f;
    public float moveSpeed = 2f;
    public bool isActive = false;

    private PlayerMove playerMoveScript;
    public float activateHeight = 1.5f;

    private Vector3 startPos;
    private bool hasActivatedPlayer = false;

    void Start()
    {
        startPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMoveScript = collision.gameObject.GetComponent<PlayerMove>();
            if (playerMoveScript != null)
            {
                playerMoveScript.playerState = PlayerState.Stay;
                hasActivatedPlayer = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMoveScript = null;
            hasActivatedPlayer = false;
        }
    }

    void Update()
    {
        if (isActive)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            transform.position = new Vector3(startPos.x, newY, startPos.z);

            if (playerMoveScript != null &&
                !hasActivatedPlayer &&
                transform.position.y >= startPos.y + activateHeight)
            {
                playerMoveScript.playerState = PlayerState.Move;
                hasActivatedPlayer = true;
                playerMoveScript = null;
            }
        }
    }
}
