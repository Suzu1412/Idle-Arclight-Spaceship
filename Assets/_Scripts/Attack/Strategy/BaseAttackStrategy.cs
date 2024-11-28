using UnityEngine;

public abstract class BaseAttackStrategy
{
    protected IAgent _agent;
    protected BaseAttackStrategySO _attackOrigin;
    protected Transform _spawnPosition;
    protected SoundDataSO _projectileSFX;


    public virtual void Initialize(IAgent agent, BaseAttackStrategySO origin, Transform spawnPosition)
    {
        _agent = agent;
        _projectileSFX = origin.ProjectileSFX;
        _attackOrigin = origin;
        _spawnPosition = spawnPosition;
    }

    public abstract void Attack(bool isPressed);
}
