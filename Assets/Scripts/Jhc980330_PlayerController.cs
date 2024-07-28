using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
using UnityEngine.Rendering;

public class Jhc980330_PlayerController : MonoBehaviour
{
    

    public float speed;
    public float jumpForce;
    public float dashForce;
    public float dashTime;
    public float dashCooldown;
    public float wallSlideSpeed;


    public Vector2 playerDirection;
    

    private float horizontal;
    private float vertical;

    public bool canDash = true;
    private bool isDashing;

    private bool isWalling;
    private bool isWallJumping;

    private bool canAttack = true;
    private bool isAttacking;
    public float attackRange = 5f;
    public float attackDuration = 0.5f;
    public float attackCooldown = 0.5f;

    private float wallJumpDirection;
    private float wallJumpTime = 0.27f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.25f;
    private Vector2 wallJumpingPower => new Vector2(speed, jumpForce);

    private float coyoteTime = 0.2f;
    private float coyoteCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private bool doubleJump = false;

    private float attackBufferTime = 0.1f;
    private float attackBufferCounter;

    Player player;

    [SerializeField]private Transform groundChk;
    [SerializeField]private LayerMask groundLayer;
    [SerializeField] private Transform wallChk1;
    [SerializeField] private Transform wallChk2;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject attackObject;
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private Transform hookObject;

    [SerializeField] CameraFollowObject _followObject;

    TrailRenderer tr;
    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        player = GetComponent<Player>();
    }


    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        TurnCheck();
        if (isWallJumping) return;
        if (isDashing) return;
        if (isGrounded())
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }
        if (wallJumpKeyInputCounter > 0)
        {
            wallJumpKeyInputCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (!isWalled() && !isDashing && canAttack && Input.GetKeyDown(KeyCode.X))
        {
            attackBufferCounter = attackBufferTime;
        }
        else
        {
            attackBufferCounter -= Time.deltaTime;
        }
        if (JumpAble()) {
            doubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
        }
        if(Input.GetKeyDown(KeyCode.Z) && rb.velocity.y>0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y *0.5f);
            coyoteCounter = 0f;
        }
        if (isWalled() && !isGrounded() && horizontal != 0 &&Input.GetKeyDown(KeyCode.Z) && wallJumpKeyInputCounter <= 0)
        {
            wallJumpKeyInputCounter = 0.02f;
        }
        if (canDash && horizontal!=0f && Input.GetKeyDown(KeyCode.C))
        {
            isDashing = true;
            StartCoroutine(Dash());
        }
        if (attackBufferCounter>0)
        {
            attackBufferCounter = 0;
            var movementInput = new Vector2(horizontal, vertical).normalized;
            if (movementInput.y != 0f)
            {
                if (movementInput.x != 0f)
                {
                    movementInput.x = 0f;
                }
            }
            if ((movementInput.x != 0f || movementInput.y != 0))
            {
                if (!(isGroundedTight() && movementInput.y < 0))
                    StartCoroutine(AttackRoutine(movementInput));
            }
            else
            {
                StartCoroutine(AttackRoutine(playerDirection));
            }
        }
        else
        {
            WallSlide();
            WallJump();
        }
    }
    private void FixedUpdate()
    {
        if (isWallJumping) return;
        if (isDashing) return;

        rb.velocity = new Vector2(horizontal*speed,rb.velocity.y);

    }
    private void TurnCheck()
    {
        if(horizontal>0 && playerDirection.x < 0)
        {
            Turn();
        }else if(horizontal<0 && playerDirection.x > 0)
        {
            Turn();
        }
    }
    private void Turn()
    {
        
        if (playerDirection.x > 0)
        {
            Vector3 rotator = new Vector3(spriteObject.transform.rotation.x,180f, spriteObject.transform.rotation.z);
            spriteObject.transform.rotation = Quaternion.Euler(rotator);
            playerDirection.x = -1f;

            _followObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(spriteObject.transform.rotation.x, 0, spriteObject.transform.rotation.z);
            spriteObject.transform.rotation = Quaternion.Euler(rotator);
            playerDirection.x = 1f;

            _followObject.CallTurn();
        }
    }
    public bool JumpAble()
    {
        return (Input.GetKeyDown(KeyCode.Z) && ((jumpBufferCounter>0f && coyoteCounter > 0f)||doubleJump));
    }
    public void doubleJumpOn()
    {
        if (!doubleJump)
        {
            doubleJump = true;
        }
    }
    public bool isGroundedTight()
    {
        return Physics2D.OverlapCircle(groundChk.position, 0.03f, groundLayer);
    }
    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundChk.position, 0.1f,groundLayer);
    }
   
    private bool isWalled()
    {
        if (isGroundedTight()) return false;
        if (Physics2D.OverlapCircle(wallChk1.position, 0.3f, wallLayer)) {
            wallJumpDirection = -1f;
            return true;
        }else if (Physics2D.OverlapCircle(wallChk2.position, 0.3f, wallLayer))
        {
            wallJumpDirection = 1f;
            return true;
        }
        wallJumpDirection = 0f;
        return false;
    }
    private void WallSlide()
    {
        if(isWalled() && !isGrounded() && horizontal !=0)
        {
            isWalling = true;
            hookObject.gameObject.SetActive(true);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWalling = false;
            hookObject.gameObject.SetActive(false);
        }
    }
    float wallJumpKeyInputCounter;
    private void WallJump()
    {
        if (isGroundedTight()) return;
        if (isWalling)
        {
            isWallJumping = false;
            wallJumpCounter = wallJumpTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }


        if (wallJumpKeyInputCounter > 0 && wallJumpCounter > 0f && wallJumpDirection!=0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpingPower.x,wallJumpingPower.y*2/3);
            wallJumpCounter = 0f;

            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    private void Attack(Vector2 vector2)
    {
        var attackTargetVector = (Vector3)this.gameObject.transform.position+ (Vector3)vector2*attackRange*(vector2.y==0? 2.5f:2f);
        var AttackSpawnPostion = new Vector2(this.transform.position.x, this.transform.position.y);
        var AttackEffect = Instantiate(attackObject, AttackSpawnPostion, Quaternion.identity);
        var AttackEffectComponent = AttackEffect.GetComponent<AttackEffect>();
        AttackEffectComponent.Set(vector2);
        AttackEffectComponent.tween = AttackEffect.transform.DOMove(attackTargetVector, attackDuration).SetEase(Ease.OutCubic).OnComplete(() => {
            Destroy(AttackEffect.gameObject); 
        });
    }
    private IEnumerator Dash()
    {
        canDash = false;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(horizontal*dashForce, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSecondsRealtime(dashCooldown);
        canDash = true;
    }
    private IEnumerator AttackRoutine(Vector2 movementInput)
    {
        rb.velocity = Vector3.zero;
        canAttack = false;
        Attack(movementInput);
        yield return new WaitForSecondsRealtime(attackCooldown);
        canAttack = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            player.Hurt(1,collision.transform.position - this.transform.position);
            Jhc980330_GameManager.Instance.GameOver();
        }   
    }

}
