using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jhc980330_JumpAgain : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color originalColor;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Jhc980330_PlayerController pc = collision.GetComponent<Jhc980330_PlayerController>();
            spriteRenderer.color = Color.yellow;
            pc.doubleJumpOn();
            Invoke(nameof(colorBack), 1f);
        }
    }

    private void colorBack()
    {
        spriteRenderer.color = originalColor;
    }
}
