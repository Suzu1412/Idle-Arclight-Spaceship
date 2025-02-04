using UnityEngine;

[CreateAssetMenu(fileName = "ChaserBossChaserAttackStateSO", menuName = "Scriptable Objects/FSM/Boss/Chaser/ChaserBossChaserAttackStateSO")]
public class ChaserBossChaserAttackStateSO : StateSO<ChaserBossContext>
{
    [SerializeField] private float _waitTime = 2f;
    [SerializeField] private AttackPatternSO _attackPattern;
    [SerializeField] private float _playerDistanceY = 3f;

    public override float EvaluateUtility(ChaserBossContext context)
    {
        return !context.HasMoveFinished ? context.ChaseMoveUtility : 0f;
    }

    public override void OnEnter(ChaserBossContext context)
    {
        context.ChaseMoveCycles++;
        context.RapidBurstCycles = 0;
        context.WallAttackCycles = 0;

        context.Repetitions = 0;
        context.WaitTimer = Time.time;
        context.ChaseState = ChaserBossContext.ChaserPatternState.Wait;
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
        if (context.Target == null) return;

        var playerPos = new Vector2(context.Target.position.x, context.Target.position.y + _playerDistanceY);
        var direction = context.Transform.position.GetDirectionTo(playerPos);
        context.Agent.Input.CallOnMovementInput(direction);

        if (context.Transform.position.GetSquaredDistanceTo(playerPos) < 0.1f)
        {
            // Movement done
            context.ChaseState = ChaserBossContext.ChaserPatternState.Attack;
            context.Agent.Input.CallOnMovementInput(Vector2.zero);
        }
    }

    private void AttackTarget(ChaserBossContext context)
    {
        context.Agent.Input.CallOnAttack(true);
        // Attack End
        context.WaitTimer = Time.time;
        context.ChaseState = ChaserBossContext.ChaserPatternState.Wait;
    }

    private void Wait(ChaserBossContext context)
    {
        context.Agent.Input.CallOnMovementInput(Vector2.zero);

        if (Time.time > context.WaitTimer + _waitTime)
        {
            // Repeat 3 times, after that the move is not selected. 
            if (context.Repetitions < 3)
            {
                context.Repetitions++;
                context.ChaseState = ChaserBossContext.ChaserPatternState.Move;
            }
            else
            {
                context.HasMoveFinished = true;
            }
        }
    }
}
