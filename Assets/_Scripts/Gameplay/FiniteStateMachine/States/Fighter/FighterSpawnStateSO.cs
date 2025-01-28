using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Fighter/Spawn State", fileName = "FighterSpawnState")]
public class FighterSpawnStateSO : StateSO<FighterContext>
{
    public override float EvaluateUtility(FighterContext context)
    {
        return !context.IsIntroExecuted ? HighestUtility : 0f;
    }

    public override void OnEnter(FighterContext context)
    {
        context.Agent.MoveBehaviour.StopMovement();
        context.IsIntroExecuted = true;
    }

    public override void OnExit(FighterContext context)
    {
    }

    public override void OnFixedUpdate(FighterContext context)
    {
    }

    public override void OnUpdate(FighterContext context)
    {
    }

    public override void HandleAttack(StateContext context, bool isAttacking)
    {
    }

    public override void HandleMovement(StateContext context, Vector2 direction)
    {
    }
}
