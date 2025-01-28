using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeSpawnStateSO", menuName = "Scriptable Objects/FSM/Kamikaze/KamikazeSpawnStateSO")]
public class KamikazeSpawnStateSO : StateSO<KamikazeContext>
{
    public override float EvaluateUtility(KamikazeContext context)
    {
        return !context.IsIntroExecuted ? HighestUtility : 0f;

    }

    public override void OnEnter(KamikazeContext context)
    {
        context.Agent.MoveBehaviour.StopMovement();
        context.Agent.Input.CallOnMovementInput(Vector2.down);
        context.IsIntroExecuted = true;
    }

    public override void OnExit(KamikazeContext context)
    {

    }

    public override void OnFixedUpdate(KamikazeContext context)
    {
    }

    public override void OnUpdate(KamikazeContext context)
    {
    }

    public override void HandleMovement(StateContext context, Vector2 direction)
    {
    }

    public override void HandleAttack(StateContext context, bool isAttacking)
    {
    }
}