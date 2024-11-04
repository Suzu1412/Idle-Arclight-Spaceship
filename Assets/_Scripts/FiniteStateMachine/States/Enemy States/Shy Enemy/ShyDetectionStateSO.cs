using UnityEngine;

public class ShyDetectionStateSO : BaseStateSO<ShyDetectionState>
{
}

public class ShyDetectionState : BaseState
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        Agent.Input.CallOnMovementInput(Vector2.down);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Agent.MoveBehaviour.Move();
    }
}
