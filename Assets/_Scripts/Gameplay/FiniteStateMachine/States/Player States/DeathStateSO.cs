using UnityEngine;

[CreateAssetMenu(fileName = "Death State", menuName = "Scriptable Objects/FSM/Player/Death State")]
public class DeathStateSO : StateSO<PlayerContext>
{
    public override float EvaluateUtility(PlayerContext context)
    {
        return context.Agent.HealthSystem.IsDeath ? HighestUtility : 0f;
    }

    public override void OnEnter(PlayerContext context)
    {
        context.Agent.HealthSystem.Remove(context.FSM.gameObject);
    }

    public override void OnExit(PlayerContext context)
    {
    }

    public override void OnFixedUpdate(PlayerContext context)
    {
    }

    public override void OnUpdate(PlayerContext context)
    {
    }
}