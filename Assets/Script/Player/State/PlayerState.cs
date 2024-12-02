using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    // Animtion StateMachine
    protected PlayerStateMachine _stateMachine;
    protected PlayerController _controller;
    private string _animatorBoolParamName;
    
    // Player Input
    protected float _xInput;
    protected float _yInput;
    protected Rigidbody2D _rigidbody2D;

    // State Timer
    protected float _stateTimer;

    public PlayerState(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName)
    { 
        this._controller = inController;
        this._stateMachine = inStateMachine;
        this._animatorBoolParamName = inParamName;
    }

    // State�� ����
    public virtual void Enter()
    {
        _controller._animtor.SetBool(_animatorBoolParamName, true);
        _rigidbody2D = _controller._rigidbody2D;
    }

    // State���� �� �����Ӹ��� ����
    public virtual void Update()
    {
        _stateTimer -= Time.deltaTime;

        _xInput = _controller._horizontalValue;
        _yInput = _controller._verticalValue;
        _controller._animtor.SetFloat("yVelocity", _rigidbody2D.velocity.y);
    }

    // State���� Ż��
    public virtual void Exit()
    {
        _controller._animtor.SetBool(_animatorBoolParamName, false);
    }
}
