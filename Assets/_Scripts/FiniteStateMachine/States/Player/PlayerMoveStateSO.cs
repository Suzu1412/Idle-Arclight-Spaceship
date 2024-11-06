using UnityEngine;

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
}
