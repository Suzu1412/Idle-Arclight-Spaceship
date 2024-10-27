using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoveStateSO", menuName = "Scriptable Objects/State/PlayerMoveStateSO")]
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
