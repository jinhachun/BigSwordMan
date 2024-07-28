using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitZone : MonoBehaviour
{
    int damageValue = 1;
    SpriteRenderer spriteRenderer;
    Player player;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void spriteOnOff(bool a)
    {
        spriteRenderer.enabled = a;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHitZone")){
            player.Hurt(damageValue,transform.position);
        }
    }
}
