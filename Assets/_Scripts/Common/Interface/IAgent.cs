using UnityEngine;
using UnityEngine.Events;

public interface IAgent
{
    IHealthSystem HealthSystem { get; }
    IAgentInput Input { get; }
    StatsSystem StatsSystem { get; }
    IAttack AttackSystem { get; }
    ICanMove MoveBehaviour { get; }
    IAgentAnimation AgentAnimation { get; }
    IAgentRenderer AgentRenderer { get; }
    ILevelSystem LevelSystem { get; }
    ITargetDetector TargetDetector { get; }
    Vector2 FacingDirection { get; }
    event UnityAction<Vector2> OnChangeFacingDirection;
    void SetFacingDirection(Vector2 direction);

    Stat GetStat(StatComponentSO statComponent);
}
