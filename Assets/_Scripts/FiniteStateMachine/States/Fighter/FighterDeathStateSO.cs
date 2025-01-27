using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FSM/Fighter/Death State", fileName = "FighterDeathState")]
public class FighterDeathStateSO : StateSO<FighterContext>
{
    public override float EvaluateUtility(FighterContext context)
    {
        return context.Agent.HealthSystem.IsDeath ? HighestUtility : 0f;
    }

    public override void OnEnter(FighterContext context)
    {
        context.Agent.HealthSystem.Remove(context.FSM.gameObject);
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
}
