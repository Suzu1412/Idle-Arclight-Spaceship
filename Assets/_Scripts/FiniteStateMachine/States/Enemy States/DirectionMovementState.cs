using UnityEngine;

[CreateAssetMenu(fileName = "DirectionMovementState", menuName = "Scriptable Objects/DirectionMovementState")]
public class DirectionMovementState : BaseStateSO<DirectionMovement>
{
    [SerializeField] private Vector2 _direction;
    public Vector2 Direction => _direction;
}

public class DirectionMovement : BaseState
{
    private Vector2 _direction;

    public override void OnEnter()
    {
        base.OnEnter();
        _direction = (_stateOrigin as DirectionMovementState).Direction;
    }

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