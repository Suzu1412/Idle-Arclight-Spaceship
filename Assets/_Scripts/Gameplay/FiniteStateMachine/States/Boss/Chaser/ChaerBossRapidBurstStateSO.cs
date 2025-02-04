using UnityEngine;

[CreateAssetMenu(fileName = "ChaerBossRapidBurstStateSO", menuName = "Scriptable Objects/FSM/Boss/Chaser/ChaerBossRapidBurstStateSO")]
public class ChaerBossRapidBurstStateSO : StateSO<ChaserBossContext>
{
    [SerializeField] private float _waitTime = 1f;
    [SerializeField] private AttackPatternSO _attackPattern1;
    [SerializeField] private AttackPatternSO _attackPattern2;
    [SerializeField] private Vector2 _position = new(0f, 4f);

    public override float EvaluateUtility(ChaserBossContext context)
    {
        return !context.HasMoveFinished ? context.RapidBurstUtility : 0f;
    }

    public override void OnEnter(ChaserBossContext context)
    {
        context.ChaseMoveCycles = 0;
        context.RapidBurstCycles++;
        context.WallAttackCycles = 0;

        context.Repetitions = 0;
        context.WaitTimer = Time.time;
        context.ChaseState = ChaserBossContext.ChaserPatternState.Move;
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
            context.ChaseState = ChaserBossContext.ChaserPatternState.Wait;
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

                if (context.Repetitions % 2 == 0)
                {
                    (context.Agent.AttackSystem as AttackSystem).SetAttackPattern(_attackPattern1);
                }
                else
                {
                    (context.Agent.AttackSystem as AttackSystem).SetAttackPattern(_attackPattern2);
                }
                context.ChaseState = ChaserBossContext.ChaserPatternState.Attack;
            }
            else
            {
                context.HasMoveFinished = true;
            }
        }
    }
}
