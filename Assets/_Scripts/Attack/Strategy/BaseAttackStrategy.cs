using UnityEngine;

public abstract class BaseAttackStrategy
{
    protected IAgent _agent;
    protected BaseAttackStrategySO _attackOrigin;
    protected Transform _spawnPosition;

    public virtual void Initialize(IAgent agent, BaseAttackStrategySO origin, Transform spawnPosition)
    {
        _agent = agent;
        _attackOrigin = origin;
        _spawnPosition = spawnPosition;
    }

    public abstract void Attack(bool isPressed);
}
