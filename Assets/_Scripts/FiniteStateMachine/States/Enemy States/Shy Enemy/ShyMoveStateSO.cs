using UnityEngine;

[CreateAssetMenu(fileName = "Shy Enemy Move State", menuName = "Scriptable Objects/State/Enemy/Shy/Move State")]
public class ShyMoveStateSO : BaseStateSO<ShyMoveState>
{
    [SerializeField] private float _offensiveMovementDuration = 6;
    [SerializeField] private float _attackMinDelay = 1;
    [SerializeField] private float _attackMaxDelay = 5;

    public float OffensiveMovementDuration => _offensiveMovementDuration;
    public float AttackMinDelay => _attackMinDelay;
    public float AttackMaxDelay => _attackMaxDelay;
}

public class ShyMoveState : BaseState
{
    private Vector2 _direction;
    private GameObject _player;
    private float _offensiveDuration;
    private float _attackMinDelay;
    private float _attackMaxDelay;
    private float _attackDelay;


    public override void OnEnter()
    {
        base.OnEnter();
        _offensiveDuration = (_stateOrigin as ShyMoveStateSO).OffensiveMovementDuration;
        _attackMinDelay = (_stateOrigin as ShyMoveStateSO).AttackMinDelay;
        _attackMaxDelay = (_stateOrigin as ShyMoveStateSO).AttackMaxDelay;
        DetectPlayer();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        _offensiveDuration -= Time.deltaTime;

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

    private void DetectPlayer()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void MoveTowardsPlayer()
    {
        if (_player == null)
        {

        }

        _direction = (_player.transform.position - _machine.transform.position).normalized;
        Agent.Input.CallOnMovementInput(_direction);
    }

    private void Attack()
    {
        if (_attackDelay <= 0f)
        {
            Agent.Input.CallOnAttack(true);
            _attackDelay = Random.Range(_attackMinDelay, _attackMaxDelay);
        }
        else
        {
            Agent.Input.CallOnAttack(false);
            _attackDelay -= Time.deltaTime;
        }
    }
}