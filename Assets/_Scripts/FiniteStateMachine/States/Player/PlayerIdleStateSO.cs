using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "Player Idle State SO", menuName = "Scriptable Objects/State/Player/Idle State SO")]
public class PlayerIdleStateSO : BaseStateSO<PlayerIdleState>
{
}

public class PlayerIdleState : BaseState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Agent.MoveBehaviour.ApplyVelocity(Vector2.zero);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Agent.PlayerDetector.IsDetected)
        {
            Agent.Input.CallOnAttack(true);
        }
    }
}