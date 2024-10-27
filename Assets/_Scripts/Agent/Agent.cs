using UnityEngine;

public class Agent : MonoBehaviour, IAgent
{
    [SerializeField] private Vector2 _facingDirection;
    private IHealthSystem _healthSystem;
    private IAgentInput _input;
    private IStatsSystem _statsSystem;
    private IAttack _attackSystem;
    private ICanMove _MoveBehaviour;
    private ILevelSystem _levelSystem;

    public IHealthSystem HealthSystem => _healthSystem ??= GetComponent<IHealthSystem>();
    public IAgentInput Input => _input ??= GetComponentInParent<IAgentInput>();
    public IStatsSystem StatsSystem => _statsSystem ??= GetComponent<IStatsSystem>();
    public IAttack AttackSystem => _attackSystem ??= GetComponent<IAttack>();
    public ICanMove MoveBehaviour => _MoveBehaviour ??= GetComponent<ICanMove>();
    public ILevelSystem LevelSystem => _levelSystem ??= GetComponent<ILevelSystem>();

    public Vector2 FacingDirection => _facingDirection;

    public float GetStat(StatType statType)
    {
        return StatsSystem.GetStatValue(statType);
    }

    public float GetStatMaxValue(StatType statType)
    {
        return StatsSystem.GetStatMaxValue(statType);
    }

    public float GetStatMinValue(StatType statType)
    {
        return StatsSystem.GetStatMinValue(statType);
    }
}
