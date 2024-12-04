using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatePrimaryAttack : PlayerState
{
    private int _comboCounter;

    private float _lastTimeAttacked;
    private float _comboWindow = 2;

    public PlayerStatePrimaryAttack(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName) 
        : base(inController, inStateMachine, inParamName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        if (_comboCounter > 2 || Time.time >= _lastTimeAttacked + _comboWindow)
            _comboCounter = 0;

        _controller._animtor.SetInteger("ComboCounter", _comboCounter);

        float _attackDir = _controller._facingDir;
        if (_xInput != 0)
            _attackDir = _xInput;

        _controller.SetVelocity(_controller._attackMovement[_comboCounter].x * _attackDir, _controller._attackMovement[_comboCounter].y);

        _stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();

        _controller.StartCoroutine("DoSomething", 0.1f);

        _comboCounter++;
        _lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (_stateTimer < 0)
            _controller.SetZeroVelocity();

        if (_triggerCalled)
            _stateMachine.ChangeState(_controller._idleState);
    }
}
