using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rigidbody;

    [Header("Collision Settings")]
    [SerializeField]
    protected float distanceToCheckGround;
    [SerializeField]
    protected Transform groundCheckPoint;
    protected bool bIsGrounded = true;

    protected bool bIsFacingRight = true;
    protected int facingDirection = 1;

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }


    protected virtual void Update()
    {
        CheckIsGrounded();
        DoFlip();
    }

    protected virtual void CheckIsGrounded()
    {
        bIsGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, distanceToCheckGround, LayerMask.GetMask("Ground"));
    }

    // Flip�� ȣ��Ǵ� ���� ȸ���ϴ� ����̹Ƿ� ���¸� ���� �� �������Ѿ� �Ѵ�
    protected virtual void Flip()
    {
        facingDirection *= -1;
        bIsFacingRight = !bIsFacingRight;
        gameObject.transform.Rotate(0, 180, 0);
    }

    protected virtual void DoFlip()
    {
        // ������ �ٶ󺸰� �����鼭 ������ Ű�� ���� ���
        if (rigidbody.velocity.x > 0 && !bIsFacingRight)
            Flip();
        // �������� �ٶ󺸰� �����鼭 ���� Ű�� ���� ���
        else if (rigidbody.velocity.x < 0 && bIsFacingRight)
            Flip();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            gameObject.transform.position,
            new Vector3(
                groundCheckPoint.position.x,
                groundCheckPoint.position.y - distanceToCheckGround)
        );
    }
}
