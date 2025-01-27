using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Fighter/Chase State", fileName = "FighterChaseState")]
public class FighterChaseStateSO : StateSO<FighterContext>
{
    public override float EvaluateUtility(FighterContext context)
    {
        return context.Agent.TargetDetector.IsDetected ? HighestUtility : 0f;
    }

    public override void OnEnter(FighterContext context)
    {
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
        if (context.Agent.TargetDetector.TargetTransform == null)
        {
            return;
        }
        var direction = Vector2.down;
        direction.x = context.FSM.transform.position.GetDirectionTo(context.Agent.TargetDetector.TargetTransform.position).x;
        context.Agent.Input.CallOnMovementInput(direction);
        context.Agent.AgentRenderer.RotateSpriteToDirection(direction);
    }
}
