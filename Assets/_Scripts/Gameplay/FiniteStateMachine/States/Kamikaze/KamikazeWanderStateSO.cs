using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeWanderStateSO", menuName = "Scriptable Objects/FSM/Kamikaze/KamikazeWanderStateSO")]
public class KamikazeWanderStateSO : StateSO<KamikazeContext>
{
    public override float EvaluateUtility(KamikazeContext context)
    {
        return !context.Agent.TargetDetector.IsDetected ? HighestUtility : 0f;

    }

    public override void OnEnter(KamikazeContext context)
    {
        var direction = context.Agent.Input.Direction; // Set on the Spawn State. Else will follow the last direction
        context.Agent.Input.CallOnMovementInput(direction);
        context.Agent.AgentRenderer.RotateSpriteToDirection(direction);
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
    }
}
