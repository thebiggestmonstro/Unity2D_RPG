using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateGrounded : PlayerState
{
    public PlayerStateGrounded(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName) 
        : base(inController, inStateMachine, inParamName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!_controller.DoDetectIsGrounded())
            _stateMachine.ChangeState(_controller._inAirState);

        if (_yInput != 0 && _controller.DoDetectIsGrounded())
            _stateMachine.ChangeState(_controller._jumpState);
    }
}
