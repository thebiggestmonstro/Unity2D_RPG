using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    InputAction moveAction;
    [SerializeField]
    InputAction jumpAction;
    [SerializeField]
    InputAction dashAction;
    [SerializeField]
    InputAction attackAction;

    [Header("Dash")]
    [SerializeField]
    float dashDuaration;
    float dashTime;
    [SerializeField]
    float dashSpeed;
    [SerializeField]
    float dashCooldown;
    float dashCooldownTimer;

    [Header("Action Parameters")]
    [SerializeField]
    float moveSpeed = 10.0f;
    [SerializeField]
    float jumpForce = 5.0f;
    [SerializeField]
    float distanceToCheckGround;

    [Header("Attack Parameters")]
    [SerializeField]
    private int comboCounter;
    [SerializeField]
    private float comboCooldown = 0.3f;
    private float comboCooldownTimer;

    Animator animator;
    Rigidbody2D rigidbody;
    
    bool bIsMoving = false;
    bool bIsGrounded = true;
    bool bIsFacingRight = true;
    bool bIsDashing = false;
    bool bIsAttacking = false;

    int facingDirection = 1;

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        dashAction.Enable();
        attackAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        dashAction.Disable();
        attackAction.Disable();
    }

    private void Update()
    {
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;

        comboCooldownTimer -= Time.deltaTime;
        
        bIsGrounded = Physics2D.Raycast(gameObject.transform.position, Vector2.down, distanceToCheckGround, LayerMask.GetMask("Ground"));
        bIsMoving = (rigidbody.velocity.x != 0);
        bIsDashing = dashTime < 0.0f ? false : true;

        UpdateMoving();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void UpdateMoving()
    {
        // Dash
        if (dashAction.IsPressed())
            DoDash();
        
        // Move 
        DoMove();
        
        // Flip
        DoFlip();

        // Jump
        if (jumpAction.IsPressed())
            DoJump();

        if (attackAction.IsPressed())
            DoAttack();
        
        // Update Parameters for Animator
        UpdateAnimatiorParameters();
    }

    void DoMove()
    {
        float horizontalValue = moveAction.ReadValue<Vector2>().x;

        if (bIsAttacking)
            rigidbody.velocity = Vector2.zero;
        else if (dashTime > 0)
            rigidbody.velocity = new Vector2(facingDirection * dashSpeed, 0);
        else
            rigidbody.velocity = new Vector2(horizontalValue * moveSpeed, rigidbody.velocity.y);
    }

    void DoJump()
    {
        if (bIsAttacking)
            return;

        if (bIsGrounded)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        }
    }

    // Flip이 호출되는 경우는 회전하는 경우이므로 상태를 전부 다 역전시켜야 한다
    void Flip()
    {
        facingDirection *= -1;
        bIsFacingRight = !bIsFacingRight;
        gameObject.transform.Rotate(0, 180, 0);
    }

    void DoFlip()
    {
        // 왼쪽을 바라보고 있으면서 오른쪽 키를 누른 경우
        if (rigidbody.velocity.x > 0 && !bIsFacingRight)
            Flip();
        // 오른쪽을 바라보고 있으면서 왼쪽 키를 누른 경우
        else if (rigidbody.velocity.x < 0 && bIsFacingRight)
            Flip();
    }

    void DoDash()
    {
        if(bIsAttacking)
            return;

        if (dashCooldownTimer < 0)
        {
            dashTime = dashDuaration;
            dashCooldownTimer = dashCooldown;
        }
    }

    void DoAttack()
    {
        if (bIsGrounded == false || bIsDashing == true)
            return;

        if (comboCooldownTimer < 0)
                comboCounter = 0;

        bIsAttacking = true;
        comboCooldownTimer = comboCooldown;
    }

    void UpdateAnimatiorParameters()
    {
        animator.SetFloat("yVelocity", rigidbody.velocity.y);
        animator.SetBool("bIsMoving", bIsMoving);
        animator.SetBool("bIsGrounded", bIsGrounded);
        animator.SetBool("bIsDashing", bIsDashing);
        animator.SetBool("bIsAttacking", bIsAttacking);
        animator.SetInteger("comboCounter", comboCounter);
    }

    public void AttackOver()
    {
        bIsAttacking = false;
        comboCounter++;

        if (comboCounter > 2)
            comboCounter = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            gameObject.transform.position, 
            new Vector3(
                gameObject.transform.position.x,
                gameObject.transform.position.y - distanceToCheckGround)
        );
    }
}
