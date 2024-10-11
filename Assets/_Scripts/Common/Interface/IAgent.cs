using UnityEngine;

public interface IAgent
{
    IHealthSystem HealthSystem { get; }
    IAgentInput Input { get; }
    IStatsSystem StatsSystem { get; }

    float GetStat(StatType statType);
    float GetStatMaxValue(StatType statType);
    float GetStatMinValue(StatType statType);
}
