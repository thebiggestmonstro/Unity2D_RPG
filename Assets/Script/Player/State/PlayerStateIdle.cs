using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerState
{
    public PlayerStateIdle(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName) 
        : base(inController, inStateMachine, inParamName)
    {

    }

    // State에 돌입
    public override void Enter()
    {
        base.Enter();
    }

    // State에서 매 프레임마다 진행
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.N))
            _controller._stateMachine.ChangeState(_controller._moveState);
    }

    // State에서 탈출
    public override void Exit()
    {
        base.Exit();
    }
}
