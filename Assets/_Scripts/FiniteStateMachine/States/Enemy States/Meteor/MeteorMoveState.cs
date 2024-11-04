using UnityEngine;

[System.Serializable]
public class MeteorMoveState : MeteorState
{
    [SerializeField] private Vector2 _direction = Vector2.down;

    public override void OnUpdate()
    {
        base.OnUpdate();
        Agent.Input.CallOnMovementInput(_direction);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Agent.MoveBehaviour.Move();
    }
}
