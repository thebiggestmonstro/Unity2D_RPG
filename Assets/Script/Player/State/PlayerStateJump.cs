using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateJump : PlayerState
{
    public PlayerStateJump(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName) 
        : base(inController, inStateMachine, inParamName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        _controller.SetVelocity(_rigidbody2D.velocity.x, _controller._jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_rigidbody2D.velocity.y < 0)
            _stateMachine.ChangeState(_controller._inAirState);
    }
}
