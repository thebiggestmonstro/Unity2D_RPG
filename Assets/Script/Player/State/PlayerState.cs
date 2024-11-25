using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine _stateMachine;
    protected PlayerController _controller;
    private string _animatorBoolParamName;

    public PlayerState(PlayerController inController, PlayerStateMachine inStateMachine, string inParamName)
    { 
        this._controller = inController;
        this._stateMachine = inStateMachine;
        this._animatorBoolParamName = inParamName;
    }

    // State�� ����
    public virtual void Enter()
    {
        Debug.Log("Enter " + _animatorBoolParamName);
    }

    // State���� �� �����Ӹ��� ����
    public virtual void Update()
    {
        Debug.Log("Update " + _animatorBoolParamName);
    }

    // State���� Ż��
    public virtual void Exit()
    {
        Debug.Log("Exit " + _animatorBoolParamName);
    }
}
