using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Player/Intro State")]
public class IntroStateSO : StateSO
{
    [SerializeField] private float _highestUtility = 999f;
    [SerializeField] private float _invulnerabilityDuration = 3f;
    public override void EnterState(FiniteStateMachine fsm)
    {
        base.EnterState(fsm);
        var context = GetContext(fsm) as IntroStateContext;
        context.HasBeenExecuted = false;
        context.Agent.HealthSystem.SetInvulnerability(true, _invulnerabilityDuration);
        context.HasBeenExecuted = true;
    }

    public override void UpdateState(FiniteStateMachine fsm)
    {
        base.UpdateState(fsm);
        var context = GetContext(fsm);
        context.UpdateTimerDeltaTime();
    }

    public override float EvaluateUtility(StateContext context)
    {
        return !(context as IntroStateContext).HasBeenExecuted ? _highestUtility  : 0; // Never Return this
    }

    public override StateContext CreateContext()
    {
        return new IntroStateContext();
    }
}

[System.Serializable]
public class IntroStateContext : StateContext
{
    public bool HasBeenExecuted = false;

    public override void HandleMovement(Vector2 direction)
    {
        base.HandleMovement(direction);
    }

    public override void HandleAttack(bool isAttacking)
    {
        base.HandleAttack(isAttacking);
    }

    public override void Reset()
    {
        Timer = 0;
    }
}