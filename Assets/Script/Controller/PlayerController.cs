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
