using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "Player Move State SO", menuName = "Scriptable Objects/State/Player/Move State SO")]
public class PlayerMoveStateSO : BaseStateSO<PlayerMoveState>
{
}

public class PlayerMoveState : BaseState
{
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        Agent.MoveBehaviour.Move();
        Agent.MoveBehaviour.BoundMovement();
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
