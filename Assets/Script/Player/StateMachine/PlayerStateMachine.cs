using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState _currentState { get; private set; }

    // 제일 처음 아무것도 아닌 상태에서 특정 State에 돌입하는 경우 호출
    public void Init(PlayerState startState)
    { 
        _currentState = startState;
        _currentState.Enter();
    }

    // 특정 State인 상태에서 다른 State에 돌입하는 경우 호출
    public void ChangeState(PlayerState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
