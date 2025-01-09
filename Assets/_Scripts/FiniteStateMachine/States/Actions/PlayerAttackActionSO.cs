using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackActionSO", menuName = "Scriptable Objects/FSM/Action/PlayerAttackActionSO")]
public class PlayerAttackActionSO : ActionSO
{
    public override bool CanExecute(FiniteStateMachine fsm)
    {
        return fsm.Agent.TargetDetector.IsDetected;
    }

    public override void Execute(FiniteStateMachine fsm)
    {
        fsm.Agent.Input.CallOnAttack(true);
    }
}
