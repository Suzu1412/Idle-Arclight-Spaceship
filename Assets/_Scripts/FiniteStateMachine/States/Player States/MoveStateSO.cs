using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Player/Move State")]
public class MoveStateSO : StateSO<MoveStateContext>
{
}


[System.Serializable]
public class MoveStateContext : StateContext<MoveStateSO>
{
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Agent.MoveBehaviour.Move();
        Agent.MoveBehaviour.BoundMovement();
    }

    public override float EvaluateUtility()
    {
        return Agent.Input.Direction != Vector2.zero ? State.HighestUtility : 0f;
    }
}
