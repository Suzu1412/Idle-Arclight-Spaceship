using UnityEngine;

[CreateAssetMenu(fileName = "Meteor Move State SO", menuName = "Scriptable Objects/State/Enemy/Meteor/Move State SO")]
public class MeteorMoveStateSO : BaseStateSO<MeteorMoveState>
{
    [SerializeField] private Vector2 _direction = Vector2.down;
    public Vector2 Direction => _direction;
}

public class MeteorMoveState : BaseState
{
    private Vector2 _direction;

    public override void OnEnter()
    {
        base.OnEnter();
        _direction = (_origin as MeteorMoveStateSO).Direction;
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
