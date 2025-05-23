using UnityEngine;
using UnityEngine.Events;

public class Agent : MonoBehaviour, IAgent
{
    [SerializeField] private Vector2 _initialFacingDirection;
    private Vector2 _facingDirection;
    private IHealthSystem _healthSystem;
    private IAgentInput _input;
    private StatsSystem _statsSystem;
    private IAttack _attackSystem;
    private ICanMove _moveBehaviour;
    private ILevelSystem _levelSystem;
    private IAgentAnimation _agentAnimation;
    private IAgentRenderer _agentRenderer;
    private ITargetDetector _targetDetector;
    private Collider2D _collider;
    private AgentDataSO _data;

    public IHealthSystem HealthSystem => _healthSystem ??= GetComponent<IHealthSystem>();
    public IAgentInput Input => _input ??= GetComponentInParent<IAgentInput>();
    public StatsSystem StatsSystem => _statsSystem ??= GetComponent<StatsSystem>();
    public IAttack AttackSystem => _attackSystem ??= GetComponent<IAttack>();
    public ICanMove MoveBehaviour => _moveBehaviour ??= GetComponent<ICanMove>();
    public ILevelSystem LevelSystem => _levelSystem ??= GetComponent<ILevelSystem>();
    public IAgentAnimation AgentAnimation => _agentAnimation ??= GetComponentInChildren<IAgentAnimation>();
    public IAgentRenderer AgentRenderer => _agentRenderer ??= GetComponentInChildren<IAgentRenderer>();
    public ITargetDetector TargetDetector => _targetDetector ??= GetComponent<ITargetDetector>();

    public event UnityAction<Vector2> OnChangeFacingDirection;
    public Vector2 FacingDirection => _facingDirection;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private async void OnEnable()
    {
        SetFacingDirection(_initialFacingDirection);
        await Awaitable.WaitForSecondsAsync(0.1f);
        //_collider.enabled = true;
    }

    private void OnDisable()
    {
        //_collider.enabled = false;
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
        HealthSystem.Initialize((int)StatsSystem.GetStat<HealthStatSO>().Value);
        AgentRenderer.SpriteRenderer.sprite = data.Sprite;
    }

    public void SetEnemyData(EnemyAgentDataSO data)
    {
        _data = data;
        MoveBehaviour.SetMoveData(data.MovementData);
        StatsSystem.SetStatsData(data.AgentStats);
        HealthSystem.Initialize((int)StatsSystem.GetStat<HealthStatSO>().Value);
        //AgentRenderer.SpriteRenderer.sprite = data.Sprite;
    }

    public Stat GetStat(StatComponentSO statComponent)
    {
        return StatsSystem.GetStat(statComponent);
    }
}
