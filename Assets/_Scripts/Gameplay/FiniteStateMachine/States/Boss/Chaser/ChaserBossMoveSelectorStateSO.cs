using UnityEngine;

[CreateAssetMenu(fileName = "ChaserBossMoveSelectorStateSO", menuName = "Scriptable Objects/FSM/Boss/Chaser/ChaserBossMoveSelectorStateSO")]
public class ChaserBossMoveSelectorStateSO : StateSO<ChaserBossContext>
{
    [SerializeField] private Vector2 _neutralPosition = new Vector2(0f, 3.5f);

    public override float EvaluateUtility(ChaserBossContext context)
    {
        return context.HasMoveFinished ? HighestUtility : 0f;
    }

    public override void OnEnter(ChaserBossContext context)
    {

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
        if (!context.CheckIfHasReachedTarget(_neutralPosition))
        {
            context.MoveTowardsTarget(_neutralPosition);
        }
        else
        {
            context.Agent.Input.CallOnMovementInput(Vector2.zero);
            context.FSM.transform.position = _neutralPosition;
            context.Agent.MoveBehaviour.RB.position = _neutralPosition;

            SelectMove(context);
        }
    }

    private void SelectMove(ChaserBossContext context)
    {
        context.ChaseMoveUtility = Random.Range(25f, 75f) - (context.ChaseMoveCycles * 25f);
        context.RapidBurstUtility = Random.Range(25f, 75f) - (context.RapidBurstCycles * 25f);
        context.WallAttackMoveUtility = Random.Range(25f, 75f) - (context.WallAttackCycles * 25f);

        context.HasMoveFinished = false;
    }
}
