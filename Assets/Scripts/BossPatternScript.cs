using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG;
using DG.Tweening;
using Cinemachine;

public class BossPatternScript : MonoBehaviour
{
    public enum BOSS_STATE { NOTONBATTLE,IDLE,MELEEATTACK,DASH,CHARGEATTACK,RANGEATTACK, RANGEATTACK_ENERGYCHARGE}

    public BOSS_STATE currentState;
    Tweener tween;
    Boss _boss;
    CinemachineImpulseSource _impulseSource;
    Transform _playerTransform;

    [SerializeField]
    Transform _sword;
    [SerializeField] GameObject chargePrefab;
    [SerializeField] GameObject rangeAttackPrefab;

    Player _player;
    public string TestCoroutineName;
    public LayerMask wallLayer;

    [SerializeField] float idleDuration = 1f;

    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashRange = 2f;

    [SerializeField] float chargeBeforeDuration = 1f;
    [SerializeField] float chargeDuration = 1f;
    [SerializeField] float chargeAfterDuration = 1f;
    [SerializeField] float chargeRange = 2f;

    [SerializeField] float attackBeforeDuration = 2f;
    [SerializeField] float attackDuration = 1f;
    [SerializeField] float attackAfterDuration = 1f;
    [SerializeField] float attackRangeX = 2f;
    [SerializeField] float attackRangeY = 2f;

    [SerializeField] float rangeAttackChargeDuration = 3f;
    [SerializeField] float rangeAttackDuration = 1f;
    [SerializeField] float rangeAttackWindDuration = .5f;
    [SerializeField] float rangeAttackWindRange = 5f;


    Vector2 originPos;
    Quaternion originRot;

    float distanceBetweenPlayer => Vector2.Distance(_playerTransform.position, this.gameObject.transform.position);

    [SerializeField] float distanceAwayFromCamera;
    [SerializeField] float distanceLong;
    [SerializeField] float distanceShort;

    private void Awake()
    {
        _boss = GetComponent<Boss>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _playerTransform = _player.transform;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void Start()
    {
        originPos = _sword.transform.localPosition;
        originRot = _sword.transform.localRotation;
        ChangeState(BOSS_STATE.NOTONBATTLE);
    }
    private void Update()
    {
        
    }
    void ChangeState(BOSS_STATE newState)
    {
        StopAllCoroutines();
        currentState = newState;
        switch (currentState)
        {
            
            case BOSS_STATE.NOTONBATTLE:
                StartCoroutine(nameof(NotOnBattleRoutine));
                break;
            case BOSS_STATE.IDLE:
                StartCoroutine(nameof(IdleRoutine));
                break;
            case BOSS_STATE.MELEEATTACK:
                StartCoroutine(nameof(MeleeAttackRoutine));
                break;
            case BOSS_STATE.DASH:
                StartCoroutine(nameof(DashRoutine));
                break;
            case BOSS_STATE.CHARGEATTACK:
                StartCoroutine(nameof(ChargeRoutine));
                break;
            case BOSS_STATE.RANGEATTACK_ENERGYCHARGE:
                StartCoroutine(nameof(RangeAttackEnergyChargeRoutine));
                break;
            case BOSS_STATE.RANGEATTACK:
                StartCoroutine(nameof(RangeAttackRoutine));
                break;
        }
    }
    [ContextMenu("코루틴 테스트")]
    public void TestCoroutine()
    {
        StartCoroutine(TestCoroutineName);
    }
    private void ResetSwordTrans()
    {
        _sword.transform.localPosition = originPos;
        _sword.transform.localRotation = originRot;
    }
    private void SetOriginSwordTrans()
    {
        originPos = _sword.transform.localPosition;
        originRot = _sword.transform.localRotation;
    }
    IEnumerator NotOnBattleRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        if(distanceBetweenPlayer < distanceLong)
        {
            _player.isBossFighting = true;
            ChangeState(BOSS_STATE.IDLE);
        }
        else
        {
            ChangeState(BOSS_STATE.NOTONBATTLE);
        }
    }
    IEnumerator DashRoutine()
    {
        var playerX = _player.transform.position.x;
        Vector3 targetPosition = this.gameObject.transform.position;
        if (_boss.isWatchingLeft)
        {
            targetPosition -= new Vector3(dashRange,0,0);
        }
        else{
            targetPosition += new Vector3(dashRange, 0, 0);
        }
        this.transform.DOMove(targetPosition,dashDuration).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(dashDuration);
        ChangeState(BOSS_STATE.MELEEATTACK);
    }
    public int attackCnt = 0;
    IEnumerator MeleeAttackRoutine()
    {
        SetOriginSwordTrans();
        _sword.transform.position = this.gameObject.transform.position + new Vector3(0,1.5f,0);
        _sword.transform.rotation = Quaternion.Euler(0, 0, 90);
        yield return new WaitForSeconds(attackBeforeDuration);
        //======================================================


        var finalAttackRangeX = attackRangeX;
        var finalRotationZ = 0f;
        if (_boss.isWatchingLeft)
        {
            finalAttackRangeX *= -1;
            finalRotationZ = 180f;
        }
        CameraShakeManager.instance.CameraShake(_impulseSource);
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(_sword.transform.DOMove(_sword.transform.position + new Vector3(finalAttackRangeX, -attackRangeY), attackDuration).SetEase(Ease.OutCubic));

        sequence.Join(_sword.transform.DORotate(new Vector3(0, 0, finalRotationZ), attackDuration).SetEase(Ease.OutCubic));
        
        yield return new WaitForSeconds(attackDuration);
        //======================================================

        yield return new WaitForSeconds(attackAfterDuration);
        ResetSwordTrans();
        attackCnt += 1;
        if(attackCnt <= 2)
        {
            ChangeState(BOSS_STATE.DASH);
        }
        else
        {
            attackCnt = 0;
            ChangeState(BOSS_STATE.IDLE);
        }
        //======================================================
    }
    IEnumerator RangeAttackEnergyChargeRoutine()
    {
        SetOriginSwordTrans();
        var originePos = this.transform.position;
        _sword.transform.position = this.gameObject.transform.position + new Vector3(0, 1.5f, 0);
        _sword.transform.rotation = Quaternion.Euler(0, 0, 90);
        var chargeParticle = Instantiate(chargePrefab, _sword.transform.position + new Vector3(0, 3.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(rangeAttackChargeDuration);
        ChangeState(BOSS_STATE.RANGEATTACK);
    }
    IEnumerator RangeAttackRoutine()
    {
        var finalAttackRangeX = attackRangeX;
        var finalRotationZ = 0f;
        if (_boss.isWatchingLeft)
        {
            finalAttackRangeX *= -1;
            finalRotationZ = 180f;
        }
        CameraShakeManager.instance.CameraShake(_impulseSource);
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(_sword.transform.DOMove(_sword.transform.position + new Vector3(finalAttackRangeX, -attackRangeY), attackDuration).SetEase(Ease.OutCubic));

        sequence.Join(_sword.transform.DORotate(new Vector3(0, 0, finalRotationZ), attackDuration).SetEase(Ease.OutCubic));
        yield return new WaitForSeconds(rangeAttackDuration);
        var attackEnergyRange = 1.5f;
        var attackPoint = _sword.transform.position + new Vector3(finalAttackRangeX* attackEnergyRange, -attackRangeY);
        for (int i=0;i< rangeAttackWindRange; i++)
        {
            CameraShakeManager.instance.CameraShake(_impulseSource);
            attackPoint += new Vector3(finalAttackRangeX, 0);
            var attackEnergy = Instantiate(rangeAttackPrefab, attackPoint, Quaternion.identity);
            attackEnergy.transform.localScale = new Vector3(attackRangeX* attackEnergyRange, 1f);
            attackEnergy.transform.DOScale(new Vector3(attackRangeX * attackEnergyRange, 10f),rangeAttackWindDuration).SetEase(Ease.InCubic).OnComplete(() => { 
                Destroy(attackEnergy.gameObject); 
            });
            yield return new WaitForSeconds(rangeAttackWindDuration);
        }
    }
    IEnumerator ChargeRoutine()
    {
        SetOriginSwordTrans();
        var originePos = this.transform.position;
        _sword.transform.position = this.gameObject.transform.position + new Vector3(_boss.isWatchingLeft ? -1 : 1f, 0f, 0);
        _sword.transform.rotation = Quaternion.Euler(0, 0, _boss.isWatchingLeft?180f:0f);
        yield return new WaitForSeconds(chargeBeforeDuration);
        //======================================================

        var finalChargeRange = chargeRange;
        RaycastHit2D hit = Physics2D.Raycast(transform.position+ new Vector3(0,1), new Vector3(_boss.isWatchingLeft?-1:1,0), chargeRange, wallLayer);
        
        if (hit.collider != null)
        {
            float distance = Vector2.Distance(transform.position, hit.point);
            if (finalChargeRange > distance)
                finalChargeRange = distance;
        }
        finalChargeRange = _boss.isWatchingLeft ? -finalChargeRange : finalChargeRange;
        var targetPos = originePos + new Vector3(finalChargeRange, 0);
        this.transform.DOMove(targetPos,chargeDuration).SetEase(Ease.InOutCubic);

        yield return new WaitForSeconds(chargeDuration);
        //======================================================
        yield return new WaitForSeconds(chargeAfterDuration);
        ResetSwordTrans();
        ChangeState(BOSS_STATE.IDLE);
        //ChangeState(CallState());
        //======================================================
    }

    IEnumerator IdleRoutine()
    {
        yield return new WaitForSeconds(idleDuration);
        _boss.TurnCheck();
        ChangeState(CallState());
    }
    [ContextMenu("다음State확인")]
    void CallStateTest()
    {
        Debug.Log(CallState());
    }
    BOSS_STATE CallState()
    {
        var randomValue = UnityEngine.Random.Range(1, 11);
        if (distanceBetweenPlayer > distanceAwayFromCamera)
        {
            return BOSS_STATE.CHARGEATTACK;
        }
        if (distanceBetweenPlayer >= distanceLong)
        {
            if (randomValue < 5)
            {
                return BOSS_STATE.RANGEATTACK_ENERGYCHARGE;
            }
            else
            {
                return BOSS_STATE.CHARGEATTACK;
            }
        }
        if (distanceBetweenPlayer >= distanceShort)
        {
            if (randomValue <= 2)
            {
                return BOSS_STATE.RANGEATTACK_ENERGYCHARGE;
            }
            else if (randomValue <= 4)
            {
                return BOSS_STATE.CHARGEATTACK;
            }
            else
            {
                return BOSS_STATE.MELEEATTACK;
            }
        }
        else
        {
            if (randomValue <= 3)
            {
                return BOSS_STATE.CHARGEATTACK;
            }
            else
            {
                return BOSS_STATE.MELEEATTACK;
            }
        }
        return BOSS_STATE.IDLE;
    }

}
[Serializable]
public class BossPattern : MonoBehaviour
{
    public string patternName;
    public float beforeDuration;
    public float duration;
    public float afterDuration;
    public IEnumerator enumerator;
}

