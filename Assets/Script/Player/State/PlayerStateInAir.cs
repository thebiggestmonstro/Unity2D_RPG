using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateInAir : PlayerState
{
    public PlayerStateInAir(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName) 
        : base(inController, inStateMachine, inParamName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (_controller.DoDetectIsGrounded())
            _stateMachine.ChangeState(_controller._idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
