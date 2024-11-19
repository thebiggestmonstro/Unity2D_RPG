using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
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

    [Header("Attack Parameters")]
    [SerializeField]
    private int comboCounter;
    [SerializeField]
    private float comboCooldown = 0.3f;
    private float comboCooldownTimer;
   
    bool bIsMoving = false;
    bool bIsDashing = false;
    bool bIsAttacking = false;

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

    protected override void Update()
    {
        base.Update();

        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;

        comboCooldownTimer -= Time.deltaTime;
        
        bIsMoving = (rigidbody.velocity.x != 0);
        bIsDashing = dashTime < 0.0f ? false : true;

        UpdateMoving();
    }

    protected override void Start()
    {
        base.Start();
    }

    void UpdateMoving()
    {
        // Dash
        if (dashAction.IsPressed())
            DoDash();
        
        // Move 
        DoMove();

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
}
