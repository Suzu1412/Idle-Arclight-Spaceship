using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Fighter/Wander State", fileName = "FighterWanderState")]
public class FighterWanderStateSO : StateSO<FighterContext>
{
    public override float EvaluateUtility(FighterContext context)
    {
        return !context.Agent.TargetDetector.IsDetected ? HighestUtility : 0f;
    }

    public override void OnEnter(FighterContext context)
    {
        var direction = Vector2.down;
        context.Agent.Input.CallOnMovementInput(direction);
        context.Agent.AgentRenderer.RotateSpriteToDirection(direction);
    }

    public override void OnExit(FighterContext context)
    {
    }

    public override void OnFixedUpdate(FighterContext context)
    {
        context.Agent.MoveBehaviour.Move();
    }

    public override void OnUpdate(FighterContext context)
    {
    }
}
