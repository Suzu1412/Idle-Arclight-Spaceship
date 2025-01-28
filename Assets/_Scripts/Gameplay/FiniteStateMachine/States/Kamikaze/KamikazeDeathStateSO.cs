using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeDeathStateSO", menuName = "Scriptable Objects/FSM/Kamikaze/KamikazeDeathStateSO")]
public class KamikazeDeathStateSO : StateSO<KamikazeContext>
{
    public override float EvaluateUtility(KamikazeContext context)
    {
        return context.Agent.HealthSystem.IsDeath ? HighestUtility : 0f;
    }

    public override void OnEnter(KamikazeContext context)
    {
        context.Agent.HealthSystem.Remove(context.FSM.gameObject);
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
}