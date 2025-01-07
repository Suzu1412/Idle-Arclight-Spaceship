using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Player/Move State")]
public class MoveStateSO : StateSO<MoveStateContext>
{
    protected override float _highestUtility => 15f;

    public override void UpdateState(FiniteStateMachine fsm)
    {
        var context = GetContext(fsm, this);

        if (context.Agent.PlayerDetector.IsDetected)
        {
            context.Agent.Input.CallOnAttack(true);
        }
    }

    public override void FixedUpdateState(FiniteStateMachine fsm)
    {
        var context = GetContext(fsm, this);
        context.Agent.MoveBehaviour.Move();
        context.Agent.MoveBehaviour.BoundMovement();
    }

    public override float EvaluateUtility(FiniteStateMachine fsm)
    {
        return fsm.Agent.Input.Direction != Vector2.zero ? _highestUtility : 0f;
    }
}


[System.Serializable]
public class MoveStateContext : StateContext
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
