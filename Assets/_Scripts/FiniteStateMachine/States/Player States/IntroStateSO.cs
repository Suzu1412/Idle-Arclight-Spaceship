using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntroStateSO", menuName = "Scriptable Objects/FSM/Player/Intro State")]
public class IntroStateSO : StateSO<IntroStateContext>
{
    [SerializeField] private float _invulnerabilityDuration = 3f;


    public float InvulnerabilityDuration => _invulnerabilityDuration;
}

[System.Serializable]
public class IntroStateContext : StateContext<IntroStateSO>
{
    [SerializeField] private bool _hasBeenExecuted = false;

    public override void OnEnter()
    {
        base.OnEnter();
        Agent.HealthSystem.SetInvulnerability(true, State.InvulnerabilityDuration);
        _hasBeenExecuted = true;
    }

    protected override void HandleMovement(Vector2 direction)
    {
    }

    protected override void HandleAttack(bool isAttacking)
    {
    }

    public override float EvaluateUtility()
    {
        return !_hasBeenExecuted ? State.HighestUtility : 0f;
    }
}