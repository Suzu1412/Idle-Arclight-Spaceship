using UnityEngine;

[System.Serializable]
public class PlayerMoveState : PlayerState
{
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        Agent.MoveBehaviour.Move();
        Agent.MoveBehaviour.BoundMovement();
    }
}
