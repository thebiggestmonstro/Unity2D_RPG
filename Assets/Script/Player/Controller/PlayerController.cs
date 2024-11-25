using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStateMachine _stateMachine { get; private set; }

    public PlayerStateIdle _idleState { get; private set; }
    public PlayerStateMove _moveState { get; private set; }

    private void Awake()
    {
        _stateMachine = new PlayerStateMachine();

        _idleState = new PlayerStateIdle(this, _stateMachine, "Idle");
        _moveState = new PlayerStateMove(this, _stateMachine, "Move");
    }

    private void Start()
    {
        _stateMachine.Init(_idleState);
    }

    private void Update()
    {
       _stateMachine._currentState.Update();
    }
}
