using UnityEngine;

[CreateAssetMenu(fileName = "ChaserBossSpawnStateSO", menuName = "Scriptable Objects/FSM/Boss/Chaser/ChaserBossSpawnStateSO")]
public class ChaserBossSpawnStateSO : StateSO<ChaserBossContext>
{
    public override float EvaluateUtility(ChaserBossContext context)
    {
        return context.IsIntroExecuted ? _highestUtility : 0f;
    }

    public override void OnEnter(ChaserBossContext context)
    {
        context.BossSpawnPosition = GameObject.FindGameObjectWithTag("BossPosition").transform.position;
        context.Agent.HealthSystem.SetInvulnerability(true, 0f, context.FSM.gameObject);

    }

    public override void OnExit(ChaserBossContext context)
    {
        context.Agent.HealthSystem.SetInvulnerability(false, 0f, context.FSM.gameObject);
    }

    public override void OnFixedUpdate(ChaserBossContext context)
    {
        context.Agent.MoveBehaviour.Move();
    }

    public override void OnUpdate(ChaserBossContext context)
    {
        if (context.BossSpawnPosition == null) return;
        var direction = context.FSM.transform.position.GetDirectionTo(context.BossSpawnPosition);

        context.Agent.Input.CallOnMovementInput(direction);

        if (context.FSM.transform.position.GetSquaredDistanceTo(context.BossSpawnPosition) < 0.2f)
        {
            context.IsIntroExecuted = true;
            context.Agent.Input.CallOnMovementInput(Vector2.zero);
            context.FSM.transform.position = context.BossSpawnPosition;
            context.Agent.MoveBehaviour.RB.position = context.BossSpawnPosition;
        }
    }
}
