using UnityEngine;

public interface IAgent
{
    IHealthSystem HealthSystem { get; }
    IAgentInput Input { get; }
    IStatsSystem StatsSystem { get; }
    IAttack AttackSystem { get; }
    ICanMove MoveBehaviour { get; }
    ILevelSystem LevelSystem { get; }
    Vector2 FacingDirection { get; }

    float GetStat(StatType statType);
    float GetStatMaxValue(StatType statType);
    float GetStatMinValue(StatType statType);
}
