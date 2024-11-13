using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum State
{ 
    Idle,
    Moving,
    Jumping,
    Falling,
    Attack,
    Die,
}

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    InputAction moveAction;
    [SerializeField]
    InputAction jumpAction;

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

    int facingDirection = 1;

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Enable();
    }

    private void Update()
    {
        UpdateMoving();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void UpdateMoving()
    {
        // Move + Flip
        float horizontalValue = moveAction.ReadValue<Vector2>().x;
        rigidbody.velocity = new Vector2(horizontalValue * moveSpeed, rigidbody.velocity.y);
        DoFlip();

        // Jump
        bIsGrounded = Physics2D.Raycast(gameObject.transform.position, Vector2.down, distanceToCheckGround, LayerMask.GetMask("Ground"));
        if (jumpAction.IsPressed() && bIsGrounded)
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        
        UpdateAnimatiorParameters();
    }

    // Flip이 호출되는 경우는 회전하는 경우이므로 상태를 전부 다 역전시켜야 한다
    void Flip()
    {
        facingDirection *= -1;
        bisFacingRight = !bisFacingRight;
        gameObject.transform.Rotate(0, 180, 0);
    }

    void DoFlip()
    {
        // 왼쪽을 바라보고 있으면서 오른쪽 키를 누른 경우
        if (rigidbody.velocity.x > 0 && !bisFacingRight)
            Flip();
        // 오른쪽을 바라보고 있으면서 왼쪽 키를 누른 경우
        else if (rigidbody.velocity.x < 0 && bisFacingRight)
            Flip();
    }

    void UpdateAnimatiorParameters()
    {
        bIsMoving = (rigidbody.velocity.x != 0);
        animator.SetFloat("yVelocity", rigidbody.velocity.y);
        animator.SetBool("bIsMoving", bIsMoving);
        animator.SetBool("bIsGrounded", bIsGrounded);
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
