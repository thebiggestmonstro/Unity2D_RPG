using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    InputAction moveAction;

    [SerializeField]
    InputAction jumpAction;

    [SerializeField]
    float moveSpeed = 10.0f;

    [SerializeField]
    float jumpForce = 5.0f; 

    Animator animator;
    Rigidbody2D rigidbody;
    bool bIsMoving;
    int facingDirection = 1;
    bool bisFacingRight = true;

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
        if (jumpAction.IsPressed())
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        }

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
        animator.SetBool("bIsMoving", bIsMoving);
    }
}
