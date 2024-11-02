using UnityEngine;
using UnityEngine.Events;

public interface IAgent
{
    IHealthSystem HealthSystem { get; }
    IAgentInput Input { get; }
    IStatsSystem StatsSystem { get; }
    IAttack AttackSystem { get; }
    ICanMove MoveBehaviour { get; }
    IAgentAnimation AgentAnimation { get; }
    IAgentRenderer AgentRenderer { get; }
    ILevelSystem LevelSystem { get; }
    Vector2 FacingDirection { get; }
    event UnityAction<Vector2> OnChangeFacingDirection;

    float GetStat(StatType statType);
    float GetStatMaxValue(StatType statType);
    float GetStatMinValue(StatType statType);
    void SetFacingDirection(Vector2 direction);
}
