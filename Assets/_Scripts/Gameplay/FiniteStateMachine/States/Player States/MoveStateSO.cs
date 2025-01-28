using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Player/Move State")]
public class MoveStateSO : StateSO<PlayerContext>
{
    public override float EvaluateUtility(PlayerContext context)
    {
        return context.Agent.Input.Direction != Vector2.zero ? HighestUtility : 0f;

    }

    public override void OnEnter(PlayerContext context)
    {
    }

    public override void OnExit(PlayerContext context)
    {
    }

    public override void OnFixedUpdate(PlayerContext context)
    {
        context.Agent.MoveBehaviour.Move();
        context.Agent.MoveBehaviour.BoundMovement();
    }

    public override void OnUpdate(PlayerContext context)
    {
    }
}