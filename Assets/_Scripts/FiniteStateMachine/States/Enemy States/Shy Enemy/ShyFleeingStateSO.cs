using UnityEngine;

[CreateAssetMenu(fileName = "Shy Enemy Fleeing State SO", menuName = "Scriptable Objects/State/Enemy/Shy/Fleeing State SO")]
public class ShyFleeingStateSO : BaseStateSO<ShyFleeingState>
{
}

public class ShyFleeingState : BaseState
{
    private Vector2 _direction;
    [SerializeField] private float _attackMaxDelay = 1.5f;
    private float _attackDelay;


    public override void OnEnter()
    {
        base.OnEnter();
        _attackDelay = _attackMaxDelay;
        SetDirection();

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Attack();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Agent.MoveBehaviour.Move();
    }

    private void SetDirection()
    {
        _direction.x = _machine.transform.position.x >= 0 ? 1 : -1;
        _direction.y = 0.5f;
        _direction = _direction.normalized;
        Agent.Input.CallOnMovementInput(_direction);

    }

    private void Attack()
    {
        if (_attackDelay <= 0f)
        {
            Agent.Input.CallOnAttack(true);
            _attackDelay = _attackMaxDelay;
        }
        else
        {
            Agent.Input.CallOnAttack(false);
            _attackDelay -= Time.deltaTime;
        }
    }
}

