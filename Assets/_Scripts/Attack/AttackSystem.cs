using UnityEngine;

public class AttackSystem : MonoBehaviour, IAttack
{
    private IAgent _agent;
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private BaseAttackStrategySO _attackStrategySO;
    private BaseAttackStrategy _attackStrategy;

    internal IAgent Agent => _agent ??= GetComponent<IAgent>();
    public Transform AttackPosition => _attackPosition;

    private void Awake()
    {
        _attackStrategy = _attackStrategySO.CreateAttack();
        _attackStrategy.Initialize(Agent, _attackStrategySO, _attackPosition);
    }

    public void Attack(bool isPressed)
    {
        _attackStrategy.Attack(isPressed);
    }
}
