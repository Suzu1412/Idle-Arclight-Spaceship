using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Player/Idle State")]
public class IdleStateSO : StateSO<IdleStateContext>
{
}

[System.Serializable]
public class IdleStateContext : StateContext<IdleStateSO>
{
    public override void OnEnter()
    {
        base.OnEnter();
        _agent.MoveBehaviour.StopMovement();
    }

    public override float EvaluateUtility()
    {
        // for boss fight use: 
        //if (context.stateMachine.currentPhase != phase)
        //{
        //    return 0f; // Ignore if not in the correct phase
        //}

        return Agent.Input.Direction == Vector2.zero ? State.HighestUtility : 0f;

    }
}