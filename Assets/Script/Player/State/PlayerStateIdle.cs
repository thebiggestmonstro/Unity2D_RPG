using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerStateGrounded
{
    public PlayerStateIdle(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName) 
        : base(inController, inStateMachine, inParamName)
    {

    }

    // State�� ����
    public override void Enter()
    {
        base.Enter();

        _rigidbody2D.velocity = Vector2.zero;
    }

    // State���� �� �����Ӹ��� ����
    public override void Update()
    {
        base.Update();

        if (_xInput != 0)
            _stateMachine.ChangeState(_controller._moveState);
    }

    // State���� Ż��
    public override void Exit()
    {
        base.Exit();
    }
}
