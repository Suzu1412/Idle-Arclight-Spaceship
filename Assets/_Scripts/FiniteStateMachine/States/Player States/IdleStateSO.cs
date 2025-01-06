using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Player/Idle State")]
public class IdleStateSO : StateSO
{
    [SerializeField] private float _maxUtility = 10f;


    public override void EnterState(FiniteStateMachine fsm)
    {
        base.EnterState(fsm);
        GetContext(fsm).Agent.MoveBehaviour.StopMovement();
    }

    public override void FixedUpdateState(FiniteStateMachine fsm)
    {
    }

    public override void UpdateState(FiniteStateMachine fsm)
    {
        var context = GetContext(fsm);

        if (context.Agent.PlayerDetector.IsDetected)
        {
            context.Agent.Input.CallOnAttack(true);
        }
    }

    public override float EvaluateUtility(StateContext context)
    {
        // for boss fight use: 
        //if (context.stateMachine.currentPhase != phase)
        //{
        //    return 0f; // Ignore if not in the correct phase
        //}

        return context.Agent.Input.Direction == Vector2.zero ? _maxUtility : 0f;
    }

    public override StateContext CreateContext()
    {
        return new IdleStateContext();
    }
}

[System.Serializable]
public class IdleStateContext : StateContext
{
    public override void HandleMovement(Vector2 direction)
    {
        base.HandleMovement(direction);
        Agent.MoveBehaviour.ReadInputDirection(direction);
    }

    public override void HandleAttack(bool isAttacking)
    {
        base.HandleAttack(isAttacking);
        Agent.AttackSystem.Attack(isAttacking);
    }

    public override void Reset()
    {
    }
}