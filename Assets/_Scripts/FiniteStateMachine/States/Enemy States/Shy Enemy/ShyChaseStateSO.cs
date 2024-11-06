using UnityEngine;

[CreateAssetMenu(fileName = "Shy Enemy Chase State SO", menuName = "Scriptable Objects/State/Enemy/Shy/Chase State SO")]
public class ShyChaseStateSO : BaseStateSO<ShyChaseState>
{
}

public class ShyChaseState : BaseState
{
    private Vector2 _direction;
    private Transform _player;
    private float _attackMaxDelay = 1.5f;
    private float _attackDelay;

    public override void OnEnter()
    {
        base.OnEnter();
        SetPlayerTransform();
        _attackDelay = _attackMaxDelay;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        MoveTowardsPlayer();
        Attack();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Agent.MoveBehaviour.Move();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void SetPlayerTransform()
    {
        _player = Agent.PlayerDetector.PlayerDetected;
    }

    private void MoveTowardsPlayer()
    {
        _direction = (_player.position - _machine.transform.position).normalized;
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