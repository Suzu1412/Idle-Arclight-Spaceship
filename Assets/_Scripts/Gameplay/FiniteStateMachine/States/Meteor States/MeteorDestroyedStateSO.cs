using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Meteor/Destroyed State")]
public class MeteorDestroyedStateSO : StateSO<MeteorContext>
{
    public override float EvaluateUtility(MeteorContext context)
    {
        return context.Agent.HealthSystem.IsDeath ? HighestUtility : 0;

    }

    public override void OnEnter(MeteorContext context)
    {
        context.Agent.HealthSystem.Remove(context.FSM.gameObject);
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