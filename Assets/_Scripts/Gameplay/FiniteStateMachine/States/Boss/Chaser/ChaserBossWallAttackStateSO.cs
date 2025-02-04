using UnityEngine;

[CreateAssetMenu(fileName = "ChaserBossWallAttackStateSO", menuName = "Scriptable Objects/FSM/Boss/Chaser/ChaserBossWallAttackStateSO")]
public class ChaserBossWallAttackStateSO : StateSO<ChaserBossContext>
{
    [SerializeField] private Vector2 _wallPosition = new(2f, 4.5f);
    [SerializeField] private AttackPatternSO _attackPattern;
    [SerializeField] private float _waitTime = 1f;

    public override float EvaluateUtility(ChaserBossContext context)
    {
        return !context.HasMoveFinished ? context.WallAttackMoveUtility : 0f;
    }

    public override void OnEnter(ChaserBossContext context)
    {
        context.ChaseMoveCycles = 0;
        context.RapidBurstCycles = 0;
        context.WallAttackCycles++;

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
        Vector2 wallPosition = new(context.Repetitions % 2 == 0 ? -_wallPosition.x : _wallPosition.x, _wallPosition.y );
        var direction = context.Transform.position.GetDirectionTo(wallPosition);
        context.Agent.Input.CallOnMovementInput(direction);

        if (context.Transform.position.GetSquaredDistanceTo(wallPosition) < 0.1f)
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
            if (context.Repetitions < 4)
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
