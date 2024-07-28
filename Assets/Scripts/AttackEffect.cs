using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public GameObject effectPrefab; // »ý¼ºÇÒ ÀÌÆåÆ® ÇÁ¸®ÆÕ
    public GameObject effectEnemyPrefab; // »ý¼ºÇÒ ÀÌÆåÆ® ÇÁ¸®ÆÕ
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public Tween tween;
    Jhc980330_PlayerController player;
    Player _player;
    Rigidbody2D playerRb;
    Vector2 attackDirection;
    CinemachineImpulseSource _CinemachineImpulseSource;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Jhc980330_PlayerController>();
        playerRb = player.GetComponent<Rigidbody2D>();
        _player = player.GetComponent<Player>();
        _CinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void Set(Vector2 vector2)
    {
        attackDirection = vector2;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        
        if (IsInLayerMask(collision.gameObject, groundLayer))
        {
            if (attackDirection.y >= 0) return;
            else
            {
                playerRb.velocity = (new Vector2(playerRb.velocity.x, player.jumpForce/2));
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("EnemyCollide");
            var enemy = collision.gameObject.GetComponent<Boss>();
            enemy.Hurt();
            _player.EarnMp();
            CreateAttackCollideEffect(effectEnemyPrefab, collision,false);
            return;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (attackDirection.y < 0)
            {
                playerRb.velocity = (new Vector2(playerRb.velocity.x, player.jumpForce / 2));
            }
        }else if (collision.gameObject.CompareTag("Lever"))
        {
            var tmpLever = collision.gameObject.GetComponent<Lever>();
            tmpLever.SetSwitchOn();
        }
        else if (IsInLayerMask(collision.gameObject, wallLayer))
        {
        }

        if (attackDirection.x < 0)
        {
            playerRb.AddForce(new Vector2(40 * player.speed, 0));
        }
        else if (attackDirection.x > 0)
        {
            playerRb.AddForce(new Vector2(40 * -player.speed, 0));
        }
        CreateAttackCollideEffect(effectPrefab, collision);
    }
    void CreateAttackCollideEffect(GameObject gameObject,Collider2D collision)
    {
        CameraShakeManager.instance.CameraShake(_CinemachineImpulseSource);
        Vector2 collisionPoint = collision.ClosestPoint(transform.position);
        Vector2 collisionDirection = (collisionPoint - (Vector2)transform.position).normalized;
        CreateEffect(gameObject, collisionPoint, collisionDirection);
        tween.Kill();
        Destroy(this.gameObject);
    }
    void CreateAttackCollideEffect(GameObject gameObject, Collider2D collision,bool isDestroy)
    {
        CameraShakeManager.instance.CameraShake(_CinemachineImpulseSource);
        Vector2 collisionPoint = collision.ClosestPoint(transform.position);
        Vector2 collisionDirection = (collisionPoint - (Vector2)transform.position).normalized;
        CreateEffect(gameObject, collisionPoint, collisionDirection);
        tween.Kill();
        if(isDestroy) 
            Destroy(this.gameObject);
    }
    bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }

    void CreateEffect(GameObject effectPrefab,Vector2 position, Vector2 direction)
    {
        GameObject effect = Instantiate(effectPrefab, position, Quaternion.identity);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        effect.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

}
