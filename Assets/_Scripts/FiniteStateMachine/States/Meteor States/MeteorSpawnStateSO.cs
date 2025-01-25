using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Spawn State")]
public class MeteorSpawnStateSO : StateSO<MeteorContext>
{
    public override float EvaluateUtility(MeteorContext context)
    {
        return !context.IsIntroExecuted ? HighestUtility : 0f;
    }

    public override void OnEnter(MeteorContext context)
    {
        context.Agent.MoveBehaviour.StopMovement();
        context.IsIntroExecuted = true;
    }

    public override void OnExit(MeteorContext context)
    {
    }

    public override void OnFixedUpdate(MeteorContext context)
    {
    }

    public override void OnUpdate(MeteorContext context)
    {
    }
}