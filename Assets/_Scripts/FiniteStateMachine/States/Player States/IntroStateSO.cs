using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntroStateSO", menuName = "Scriptable Objects/FSM/Player/Intro State")]
public class IntroStateSO : StateSO<IntroStateContext>
{
    [SerializeField] private float _invulnerabilityDuration = 3f;
    [SerializeField] private GameObjectRuntimeSetSO _playerRTS;


    public float InvulnerabilityDuration => _invulnerabilityDuration;
    public GameObjectRuntimeSetSO PlayerRTS => _playerRTS;
}

[System.Serializable]
public class IntroStateContext : StateContext<IntroStateSO>
{
    [SerializeField] private bool _hasBeenExecuted = false;

    public override void OnEnter()
    {
        base.OnEnter();
        State.PlayerRTS.Add(_fsm.gameObject);
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