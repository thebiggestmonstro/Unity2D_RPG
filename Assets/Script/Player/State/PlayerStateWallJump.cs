using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWallJump : PlayerState
{
    public PlayerStateWallJump(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName) 
        : base(inController, inStateMachine, inParamName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        _stateTimer = 0.1f;
        _controller.SetVelocity(5 * -_controller._facingDir, _controller._jumpForce);

    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if (_stateTimer < 0)
            _stateMachine.ChangeState(_controller._inAirState);

        if (_controller.DoDetectIsGrounded())
            _stateMachine.ChangeState(_controller._idleState);
    }
}