using UnityEngine;

[CreateAssetMenu(fileName = "FighterAttackActionSO", menuName = "Scriptable Objects/FSM/Action/FighterAttackActionSO")]
public class FighterAttackActionSO : ActionSO<FighterContext>
{
    public override bool CanExecute(FighterContext context)
    {
        return context.Agent.TargetDetector.IsVisibleToCamera && context.CanAttack();
    }

    public override void DrawGizmos(FighterContext context)
    {
    }

    public override void Execute(FighterContext context)
    {
        context.Agent.Input.CallOnAttack(true);
    }

}