using UnityEngine;
using UnityEngine.PlayerLoop;

public class AttackSystem : MonoBehaviour, IAttack
{
    private IAgent _agent;
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private BaseAttackStrategySO _attackStrategySO;
    [SerializeField] private LayerMask _projectileMask;
    private BaseAttackStrategy _attackStrategy;
    private float _attackDelay;
    internal IAgent Agent => _agent ??= GetComponent<IAgent>();
    public Transform AttackPosition => _attackPosition;

    public LayerMask ProjectileMask => _projectileMask;

    private void Awake()
    {
        _attackStrategy = _attackStrategySO.CreateAttack();
        _attackStrategy.Initialize(Agent, _attackStrategySO, _attackPosition);
    }

    private void Update()
    {
        _attackDelay -= Time.deltaTime;
    }

    public void Attack(bool isPressed)
    {
        if (_attackDelay > 0f) return;
        _attackStrategy.Attack(isPressed);
        _attackDelay = _attackStrategySO.AttackDelay;
    }
}
