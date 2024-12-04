using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWallSlide : PlayerState
{
    public PlayerStateWallSlide(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName) 
        : base(inController, inStateMachine, inParamName)
    {

    }

    // State에 돌입
    public override void Enter()
    {
        base.Enter();

        
    }

    // State에서 탈출
    public override void Exit()
    {
        base.Exit();
    }

    // State에서 매 프레임마다 진행
    public override void Update()
    {
        base.Update();

        if (_isJumping)
        {
            _stateMachine.ChangeState(_controller._wallJumpState);
            return;
        }

        if (_xInput != 0 && _controller._facingDir != _xInput)
            _stateMachine.ChangeState(_controller._idleState);

        if (_yInput < 0.0f)
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        else
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y * 0.7f);

        if (_controller.DoDetectIsGrounded())
            _stateMachine.ChangeState(_controller._idleState);
    }
}
