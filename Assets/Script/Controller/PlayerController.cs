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

    [Header("Dash")]
    [SerializeField]
    float dashDuaration;
    float dashTime;
    [SerializeField]
    float dashSpeed;
    [SerializeField]
    float dashCooldown;
    float dashCooldownTimer;

    [Header("Parameters")]
    [SerializeField]
    float moveSpeed = 10.0f;
    [SerializeField]
    float jumpForce = 5.0f;
    [SerializeField]
    float distanceToCheckGround;

    Animator animator;
    Rigidbody2D rigidbody;
    
    bool bIsMoving = false;
    bool bIsGrounded = true;
    bool bisFacingRight = true;
    bool bIsDashing = false;

    int facingDirection = 1;

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        dashAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        dashAction.Disable();
    }

    private void Update()
    {
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;

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
        if (dashAction.IsPressed() && dashCooldownTimer < 0)
            DoDash();
        
        // Move 
        DoMove();
        
        // Flip
        DoFlip();

        // Jump
        if (jumpAction.IsPressed() && bIsGrounded)
            DoJump();
        
        // Update Parameters for Animator
        UpdateAnimatiorParameters();
    }

    void DoMove()
    {
        float horizontalValue = moveAction.ReadValue<Vector2>().x;

        if (dashTime > 0)
            rigidbody.velocity = new Vector2(horizontalValue * dashSpeed, 0);
        else
            rigidbody.velocity = new Vector2(horizontalValue * moveSpeed, rigidbody.velocity.y);
    }

    void DoJump()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
    }

    // Flip�� ȣ��Ǵ� ���� ȸ���ϴ� ����̹Ƿ� ���¸� ���� �� �������Ѿ� �Ѵ�
    void Flip()
    {
        facingDirection *= -1;
        bisFacingRight = !bisFacingRight;
        gameObject.transform.Rotate(0, 180, 0);
    }

    void DoFlip()
    {
        // ������ �ٶ󺸰� �����鼭 ������ Ű�� ���� ���
        if (rigidbody.velocity.x > 0 && !bisFacingRight)
            Flip();
        // �������� �ٶ󺸰� �����鼭 ���� Ű�� ���� ���
        else if (rigidbody.velocity.x < 0 && bisFacingRight)
            Flip();
    }

    void DoDash()
    {
        dashTime = dashDuaration;
        dashCooldownTimer = dashCooldown;
    }

    void UpdateAnimatiorParameters()
    {
        animator.SetFloat("yVelocity", rigidbody.velocity.y);
        animator.SetBool("bIsMoving", bIsMoving);
        animator.SetBool("bIsGrounded", bIsGrounded);
        animator.SetBool("bIsDashing", bIsDashing);
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
