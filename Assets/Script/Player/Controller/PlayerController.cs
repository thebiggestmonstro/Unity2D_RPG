using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Burst;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Animtion StateMachine
    public Animator _animtor { get; private set; }
    public PlayerStateMachine _stateMachine { get; private set; }

    #region States
    public PlayerStateIdle _idleState { get; private set; }
    public PlayerStateMove _moveState { get; private set; }
    public PlayerStateJump _jumpState { get; private set; }
    public PlayerStateInAir _inAirState { get; private set; }
    public PlayerStateDash _dashState { get; private set; }
    public PlayerStateWallSlide _wallSlideState { get; private set; }
    public PlayerStateWallJump _wallJumpState { get; private set; }
    public PlayerStatePrimaryAttack _priamaryAttackState { get; private set; }
    #endregion

    // Player Input
    [SerializeField]
    InputAction _moveAction;
    [SerializeField]
    InputAction _jumpAction;
    [SerializeField]
    InputAction _dashAction;
    [SerializeField]
    InputAction _attackAction;

    // Player Move Info
    public float _moveSpeed = 12f;
    public float _jumpForce = 12f;
    public bool _isJumpPressed;
    public bool _isAttackClicked;
    public float _horizontalValue { get; private set; }
    public float _verticalValue { get; private set; }

    // Dash Info
    [SerializeField]
    private float _dashCooldown;
    private float _dashUsageTimer;
    public float _dashSpeed;
    public float _dashDuration;
    public float _dashDir { get; private set; }

    public bool _doingSomething { get; private set; }
    public Rigidbody2D _rigidbody2D { get; private set; }

    [Header("Collision Info")]
    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private float _groundCheckDistance;
    [SerializeField]
    private Transform _wallCheck;
    [SerializeField]
    private float _wallCheckDistance;

    [Header("Attack Details")]
    public Vector2[] _attackMovement;

    public int _facingDir { get; private set; } = 1;
    private bool _facingRight = true;

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _dashAction.Enable();
        _attackAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _dashAction.Disable();
        _attackAction.Disable();
    }

    private void Awake()
    {
        _stateMachine = new PlayerStateMachine();

        _idleState = new PlayerStateIdle(this, _stateMachine, "Idle");
        _moveState = new PlayerStateMove(this, _stateMachine, "Move");
        _jumpState = new PlayerStateJump(this, _stateMachine, "Jump");
        _inAirState = new PlayerStateInAir(this, _stateMachine, "Jump");
        _dashState = new PlayerStateDash(this, _stateMachine, "Dash");
        _wallSlideState = new PlayerStateWallSlide(this, _stateMachine, "WallSlide");
        _wallJumpState = new PlayerStateWallJump(this, _stateMachine, "WallJump");
        _priamaryAttackState = new PlayerStatePrimaryAttack(this, _stateMachine, "Attack");
    }

    private void Start()
    {
        _animtor = GetComponentInChildren<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _stateMachine.Init(_idleState);
    }

    private void Update()
    {
        _stateMachine._currentState.Update();

        DoMove();

        DoJump();

        DoDash();

        DoAttack();
    }

    public IEnumerator DoSomething(float _seconds)
    {
        _doingSomething = true;

        yield return new WaitForSeconds(_seconds);

        _doingSomething = false;
    }

    public void SetVelocity(float xVelocity, float yVelcoity)
    {
        _rigidbody2D.velocity = new Vector2(xVelocity, yVelcoity);
        DoFlip(xVelocity);
    }

    public void SetZeroVelocity() => _rigidbody2D.velocity = Vector2.zero; 

    void DoMove()
    {
        _horizontalValue = _moveAction.ReadValue<Vector2>().x;
        _verticalValue = _moveAction.ReadValue<Vector2>().y;
    }

    void DoJump()
    {
        if (_jumpAction.IsPressed())
            _isJumpPressed = true;
        else
            _isJumpPressed = false;
    }

    public void Flip()
    {
        _facingDir *= -1;
        _facingRight = !_facingRight;
        gameObject.transform.Rotate(0, 180, 0);
    }

    void DoFlip(float xParam)
    {
        if (xParam > 0 && !_facingRight)
            Flip();
        else if (xParam < 0 && _facingRight)
            Flip();
    }

    void DoDash()
    {
        if (DoDetectIsFacingWall())
            return;

        _dashUsageTimer -= Time.deltaTime;

        if (_dashAction.IsPressed() && _dashUsageTimer < 0)
        {
            _dashUsageTimer = _dashCooldown;
            _dashDir = _moveAction.ReadValue<Vector2>().x;

            if (_dashDir == 0)
                _dashDir = _facingDir;

            _stateMachine.ChangeState(_dashState);
        }       
    }

    void DoAttack()
    {
        if (_attackAction.IsPressed())
            _isAttackClicked = true;
        else
            _isAttackClicked = false;
    }

    public void AnimationTrigger() => _stateMachine._currentState.AnimationFinishTrigger();

    public bool DoDetectIsGrounded() => Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, LayerMask.GetMask("Ground"));
    public bool DoDetectIsFacingWall() => Physics2D.Raycast(_wallCheck.position, Vector2.right * _facingDir, _wallCheckDistance, LayerMask.GetMask("Ground"));

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            _groundCheck.position,
            new Vector3(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance)
        );

        Gizmos.DrawLine(
            _wallCheck.position,
            new Vector3(_wallCheck.position.x + _wallCheckDistance, _wallCheck.position.y)
        );
    }
}
