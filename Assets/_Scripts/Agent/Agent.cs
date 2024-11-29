using UnityEngine;
using UnityEngine.Events;

public class Agent : MonoBehaviour, IAgent
{
    [SerializeField] private Vector2 _initialFacingDirection;
    private Vector2 _facingDirection;
    private IHealthSystem _healthSystem;
    private IAgentInput _input;
    private IStatsSystem _statsSystem;
    private IAttack _attackSystem;
    private ICanMove _moveBehaviour;
    private ILevelSystem _levelSystem;
    private IAgentAnimation _agentAnimation;
    private IAgentRenderer _agentRenderer;
    private IPlayerDetector _playerDetector;
    private AgentDataSO _data;

    public IHealthSystem HealthSystem => _healthSystem ??= GetComponent<IHealthSystem>();
    public IAgentInput Input => _input ??= GetComponentInParent<IAgentInput>();
    public IStatsSystem StatsSystem => _statsSystem ??= GetComponent<IStatsSystem>();
    public IAttack AttackSystem => _attackSystem ??= GetComponent<IAttack>();
    public ICanMove MoveBehaviour => _moveBehaviour ??= GetComponent<ICanMove>();
    public ILevelSystem LevelSystem => _levelSystem ??= GetComponent<ILevelSystem>();
    public IAgentAnimation AgentAnimation => _agentAnimation ??= GetComponentInChildren<IAgentAnimation>();
    public IAgentRenderer AgentRenderer => _agentRenderer ??= GetComponentInChildren<IAgentRenderer>();
    public IPlayerDetector PlayerDetector => _playerDetector ??= GetComponent<IPlayerDetector>();

    public event UnityAction<Vector2> OnChangeFacingDirection;
    public Vector2 FacingDirection => _facingDirection;


    private void OnEnable()
    {
        SetFacingDirection(_initialFacingDirection);
    }


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

    public void SetFacingDirection(Vector2 direction)
    {
        _facingDirection = direction;
        OnChangeFacingDirection?.Invoke(direction);
    }

    public void SetPlayerData(PlayerAgentDataSO data)
    {
        _data = data;
        MoveBehaviour.SetMoveData(data.MovementData);
        StatsSystem.SetStatsData(data.AgentStats);
        HealthSystem.Initialize((int)GetStat(StatType.MaxHealth));
        AgentRenderer.SpriteRenderer.sprite = data.Sprite;
    }

    public void SetEnemyData(EnemyAgentDataSO data)
    {
        _data = data;
        MoveBehaviour.SetMoveData(data.MovementData);
        StatsSystem.SetStatsData(data.AgentStats);
        HealthSystem.Initialize((int)GetStat(StatType.MaxHealth));
        //AgentRenderer.SpriteRenderer.sprite = data.Sprite;
    }
}
