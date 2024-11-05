using UnityEngine;

[System.Serializable]
public class ShyWanderingState : ShyEnemyState
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
