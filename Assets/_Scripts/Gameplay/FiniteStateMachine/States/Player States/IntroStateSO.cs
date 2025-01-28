using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntroStateSO", menuName = "Scriptable Objects/FSM/Player/Intro State")]
public class IntroStateSO : StateSO<PlayerContext>
{
    [SerializeField] private float _invulnerabilityDuration = 3f;


    public override float EvaluateUtility(PlayerContext context)
    {
        return !context.IsIntroExecuted ? HighestUtility : 0f;
    }

    public override void OnEnter(PlayerContext context)
    {
        context.Agent.HealthSystem.SetInvulnerability(true, _invulnerabilityDuration);
        context.IsIntroExecuted = true;
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