using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
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
    #endregion

    // Player Input
    [SerializeField]
    InputAction _moveAction;
    [SerializeField]
    InputAction _jumpAction;
    [SerializeField]
    InputAction _dashAction;

    // Player Move Info
    public float _moveSpeed = 12f;
    public float _jumpForce = 12f;
    public float _horizontalValue  { get; private set; }
    public float _verticalValue { get; private set; }

    // Dash Info
    [SerializeField]
    private float _dashCooldown;
    private float _dashUsageTimer;
    public float _dashSpeed;
    public float _dashDuration;
    public float _dashDir { get; private set; } 

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


    public int _facingDir { get; private set; } = 1;
    private bool _facingRight = true;

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _dashAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _dashAction.Disable();
    }

    private void Awake()
    {
        _stateMachine = new PlayerStateMachine();

        _idleState = new PlayerStateIdle(this, _stateMachine, "Idle");
        _moveState = new PlayerStateMove(this, _stateMachine, "Move");
        _jumpState = new PlayerStateJump(this, _stateMachine, "Jump");
        _inAirState = new PlayerStateInAir(this, _stateMachine, "Jump");
        _dashState = new PlayerStateDash(this, _stateMachine, "Dash");
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
    }

    public void SetVelocity(float xVelocity, float yVelcoity)
    {
        _rigidbody2D.velocity = new Vector2(xVelocity, yVelcoity);
        DoFlip(xVelocity);
    }

    void DoMove()
    {
        _horizontalValue = _moveAction.ReadValue<Vector2>().x;
    }

    void DoJump()
    {
        if (_jumpAction.IsPressed())
            _verticalValue = _jumpForce;
        else
            _verticalValue = 0.0f;
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

    public bool DoDetectIsGrounded() => Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, LayerMask.GetMask("Ground"));

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
