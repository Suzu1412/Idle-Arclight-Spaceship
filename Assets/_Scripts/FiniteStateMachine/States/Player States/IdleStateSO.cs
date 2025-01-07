using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Player/Idle State")]
public class IdleStateSO : StateSO<IdleStateContext>
{
    protected override float _highestUtility => 10f;

    public override void EnterState(FiniteStateMachine fsm)
    {
        base.EnterState(fsm);
        fsm.Agent.MoveBehaviour.StopMovement();
    }

    public override void FixedUpdateState(FiniteStateMachine fsm)
    {
    }

    public override void UpdateState(FiniteStateMachine fsm)
    {
        var context = GetContext(fsm, this);

        if (context.Agent.PlayerDetector.IsDetected)
        {
            context.Agent.Input.CallOnAttack(true);
        }
    }

    public override float EvaluateUtility(FiniteStateMachine fsm)
    {
        // for boss fight use: 
        //if (context.stateMachine.currentPhase != phase)
        //{
        //    return 0f; // Ignore if not in the correct phase
        //}


        return fsm.Agent.Input.Direction == Vector2.zero ? _highestUtility : 0f;
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