using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerState
{
    public PlayerStateIdle(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName) 
        : base(inController, inStateMachine, inParamName)
    {

    }

    // State�� ����
    public override void Enter()
    {
        base.Enter();
    }

    // State���� �� �����Ӹ��� ����
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.N))
            _controller._stateMachine.ChangeState(_controller._moveState);
    }

    // State���� Ż��
    public override void Exit()
    {
        base.Exit();
    }
}
