using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Collider2D _collider2d;
    CinemachineImpulseSource _impulseSource;
    Jhc980330_PlayerController _playerController;
    SpriteRenderer[] renderers;

    public bool isBossFighting = false;
    public int Hp;
    public int MHp;
    public int Mp;
    public int MMp;
    public int Att;

    [SerializeField] private GameObject HurtEffect;
    [SerializeField] private bool isHurt;
    [SerializeField] private bool isKnockBack;
    [SerializeField] private float noHitTime;
    [SerializeField] private Color32 hurtColor = Color.red;
    private List<Color32> fullColor;


    private void Awake()
    {
        this._playerController = GetComponent<Jhc980330_PlayerController>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        fullColor = new List<Color32>();
        foreach (var renderer in renderers)
            fullColor.Add(renderer.color);
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void EarnMp()
    {
        Mp++;
    }
    public void Heal()
    {
        Hp++;
    }
    [ContextMenu("¾ÆÇÁ±â")]
    public void Hurt()
    {
        Hurt(1, Vector2.left);
    }
    public void Hurt(int damage,Vector2 pos)
    {
        _collider2d.enabled = false;
        if (!isHurt)
        {
            CameraShakeManager.instance.CameraShake(_impulseSource);
            isHurt = true;
            Hp -= damage;
            if (Hp < 0)
            {

            }
            else
            {
                var hurtEffect = Instantiate(HurtEffect, this.transform.position, Quaternion.identity);
                hurtEffect.transform.localScale *= 0.5f;
                float x = transform.position.x;
                if (x < 0) x = 1; else x = -1;
                StartCoroutine(Knockback(x));
                StartCoroutine(HurtRoutine());
                StartCoroutine(AlphaBlink());
            }
        }
    }
    IEnumerator HurtRoutine()
    {
        this.gameObject.layer = LayerMask.NameToLayer("HurtPlayer");
        yield return new WaitForSeconds(noHitTime);
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        _collider2d.enabled = true;
        isHurt = false;
    }
    IEnumerator AlphaBlink()
    {
        while (isHurt)
        {
            yield return new WaitForSeconds(0.1f);
            foreach(SpriteRenderer renderer in renderers)
            {
                renderer.color = hurtColor;
            }
            yield return new WaitForSeconds(0.1f);
            int i = 0;
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.color = fullColor[i];
                i++;
            }

        }
    }
    IEnumerator Knockback(float dir)
    {
        isKnockBack = true;
        float ctime = 0f;
        while (ctime < 0.2f)
        {
            if(transform.rotation.y==0)
                transform.Translate(Vector2.left *  Time.deltaTime * dir);
            else
                transform.Translate(Vector2.left *  Time.deltaTime * dir * -1f);
            ctime += Time.deltaTime;
            yield return null;

        }
        isKnockBack = false;
    }
}
