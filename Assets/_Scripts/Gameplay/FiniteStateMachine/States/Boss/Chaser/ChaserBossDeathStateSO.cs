using UnityEngine;

[CreateAssetMenu(fileName = "ChaserBossDeathStateSO", menuName = "Scriptable Objects/FSM/Boss/Chaser/ChaserBossDeathStateSO")]
public class ChaserBossDeathStateSO : StateSO<ChaserBossContext>
{
    public override float EvaluateUtility(ChaserBossContext context)
    {
        return context.IsDeath ? HighestUtility : 0f;
    }

    public override void OnEnter(ChaserBossContext context)
    {
        context.HandleDeath();
    }

    public override void OnExit(ChaserBossContext context)
    {
    }

    public override void OnFixedUpdate(ChaserBossContext context)
    {
    }

    public override void OnUpdate(ChaserBossContext context)
    {
    }
}
