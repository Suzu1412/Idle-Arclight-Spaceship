using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeChaseStateSO", menuName = "Scriptable Objects/FSM/Kamikaze/KamikazeChaseStateSO")]
public class KamikazeChaseStateSO : StateSO<KamikazeContext>
{
    public override float EvaluateUtility(KamikazeContext context)
    {
        return context.Agent.TargetDetector.IsDetected ? HighestUtility : 0f;
    }

    public override void OnEnter(KamikazeContext context)
    {
    }

    public override void OnExit(KamikazeContext context)
    {
    }

    public override void OnFixedUpdate(KamikazeContext context)
    {
        context.Agent.MoveBehaviour.Move();
    }

    public override void OnUpdate(KamikazeContext context)
    {
        if (context.Agent.TargetDetector.TargetTransform == null)
        {
            return;
        }
        var direction = context.FSM.transform.position.GetDirectionTo(context.Agent.TargetDetector.TargetTransform.position);
        context.Agent.Input.CallOnMovementInput(direction);
        context.Agent.AgentRenderer.RotateSpriteToDirection(direction);
    }
}