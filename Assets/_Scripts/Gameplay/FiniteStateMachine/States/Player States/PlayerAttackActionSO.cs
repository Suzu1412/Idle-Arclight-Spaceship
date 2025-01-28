using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackActionSO", menuName = "Scriptable Objects/FSM/Action/PlayerAttackActionSO")]
public class PlayerAttackActionSO : ActionSO<PlayerContext>
{
    public override bool CanExecute(PlayerContext context)
    {
        return context.Agent.TargetDetector.IsDetected;
    }

    public override void DrawGizmos(PlayerContext context)
    {
    }

    public override void Execute(PlayerContext context)
    {
        context.Agent.Input.CallOnAttack(true);
    }
}
