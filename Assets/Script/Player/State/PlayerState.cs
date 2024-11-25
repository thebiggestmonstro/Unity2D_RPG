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

    // State에 돌입
    public virtual void Enter()
    {
        Debug.Log("Enter " + _animatorBoolParamName);
    }

    // State에서 매 프레임마다 진행
    public virtual void Update()
    {
        Debug.Log("Update " + _animatorBoolParamName);
    }

    // State에서 탈출
    public virtual void Exit()
    {
        Debug.Log("Exit " + _animatorBoolParamName);
    }
}
