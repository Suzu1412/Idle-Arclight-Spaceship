using UnityEngine;

[CreateAssetMenu(fileName = "ChaserBossBerserkStateSO", menuName = "Scriptable Objects/FSM/Boss/Chaser/ChaserBossBerserkStateSO")]
public class ChaserBossBerserkStateSO : StateSO<ChaserBossContext>
{
    [SerializeField] private float _waitTime = 1f;
    [SerializeField] private AttackPatternSO _attackPattern;
    [SerializeField] private Vector2 _position = Vector2.zero;

    public override float EvaluateUtility(ChaserBossContext context)
    {
        return context.HealthPercent <= 0.33f ? _highestUtility : 0f;
    }

    public override void OnEnter(ChaserBossContext context)
    {
        context.ChaseState = ChaserBossContext.ChaserPatternState.Move;
        (context.Agent.AttackSystem as AttackSystem).SetAttackPattern(_attackPattern);
    }

    public override void OnExit(ChaserBossContext context)
    {
    }

    public override void OnFixedUpdate(ChaserBossContext context)
    {
        context.Move();
    }

    public override void OnUpdate(ChaserBossContext context)
    {
        CurrentMove(context);
    }

    private void CurrentMove(ChaserBossContext context)
    {
        switch (context.ChaseState)
        {
            case ChaserBossContext.ChaserPatternState.Move:
                MoveToTarget(context);
                break;

            case ChaserBossContext.ChaserPatternState.Attack:
                AttackTarget(context);
                break;

            case ChaserBossContext.ChaserPatternState.Wait:
                Wait(context);
                break;
        }
    }

    private void MoveToTarget(ChaserBossContext context)
    {
        var direction = context.Transform.position.GetDirectionTo(_position);
        context.Agent.Input.CallOnMovementInput(direction);

        if (context.Transform.position.GetSquaredDistanceTo(_position) < 0.1f)
        {
            // Movement done
            context.ChaseState = ChaserBossContext.ChaserPatternState.Attack;
            context.Agent.Input.CallOnMovementInput(Vector2.zero);
        }
    }

    private void AttackTarget(ChaserBossContext context)
    {
        context.Agent.Input.CallOnAttack(true);
        context.WaitTimer = Time.time;
        context.ChaseState = ChaserBossContext.ChaserPatternState.Wait;
    }

    private void Wait(ChaserBossContext context)
    {
        context.Agent.Input.CallOnMovementInput(Vector2.zero);

        if (Time.time > context.WaitTimer + _waitTime)
        {
                context.ChaseState = ChaserBossContext.ChaserPatternState.Attack;
    
        }
    }
}
