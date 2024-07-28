using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayWall : MonoBehaviour
{
    private Collider2D wallCollider;

    public Vector2 allowedDirection = Vector2.right; // 예: 위쪽에서 아래쪽으로만 통과 가능

    void Start()
    {
        wallCollider = GetComponent<Collider2D>();
        if (!wallCollider)
        {
            Debug.LogError("Collider2D not found on the wall object.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector2 directionToPlayer = other.transform.position - transform.position;

            if (Vector2.Dot(directionToPlayer, allowedDirection) > 0)
            {
                wallCollider.isTrigger = true;
            }
            else
            {
                wallCollider.isTrigger = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wallCollider.isTrigger = true;
        }
    }
}