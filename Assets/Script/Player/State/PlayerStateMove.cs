using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMove : PlayerStateGrounded
{
    public PlayerStateMove(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName) 
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

        _controller.SetVelocity(_xInput * _controller._moveSpeed, _rigidbody2D.velocity.y);

        if (_xInput == 0 || _controller.DoDetectIsFacingWall())
            _stateMachine.ChangeState(_controller._idleState);
    }
}
