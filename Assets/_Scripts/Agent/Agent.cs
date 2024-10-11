using UnityEngine;

public class Agent : MonoBehaviour, IAgent
{
    private IHealthSystem _healthSystem;
    private IAgentInput _input;
    private IStatsSystem _statsSystem;

    public IHealthSystem HealthSystem => _healthSystem ??= GetComponent<IHealthSystem>();
    public IAgentInput Input => _input ??= GetComponentInParent<IAgentInput>();
    public IStatsSystem StatsSystem => _statsSystem ??= GetComponent<IStatsSystem>();


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
