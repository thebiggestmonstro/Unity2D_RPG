using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerStateGrounded
{
    public PlayerStateIdle(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName) 
        : base(inController, inStateMachine, inParamName)
    {

    }

    // State에 돌입
    public override void Enter()
    {
        base.Enter();

        _controller.SetZeroVelocity();
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

        if (_xInput == _controller._facingDir && _controller.DoDetectIsFacingWall())
            return;

        if (_xInput != 0 && !_controller._doingSomething)
            _stateMachine.ChangeState(_controller._moveState);
    }
}
