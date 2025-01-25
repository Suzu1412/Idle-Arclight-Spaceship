using JetBrains.Annotations;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Player/Idle State")]
public class IdleStateSO : StateSO<PlayerContext>
{
    public override float EvaluateUtility(PlayerContext context)
    {
        return context.Agent.Input.Direction == Vector2.zero ? HighestUtility : 0f;

    }

    public override void OnEnter(PlayerContext context)
    {
        context.Agent.MoveBehaviour.StopMovement();
    }

    public override void OnExit(PlayerContext context)
    {
    }

    public override void OnFixedUpdate(PlayerContext context)
    {
    }

    public override void OnUpdate(PlayerContext context)
    {
    }
}

