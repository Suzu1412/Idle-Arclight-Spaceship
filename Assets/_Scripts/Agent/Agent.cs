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
    private FiniteStateMachine _fsm;
    private Collider2D _collider;
    private DropItem _dropItem;

    public IHealthSystem HealthSystem => _healthSystem ??= GetComponent<IHealthSystem>();
    public IAgentInput Input => _input ??= GetComponentInParent<IAgentInput>();
    public FiniteStateMachine FSM => _fsm = _fsm != null ? _fsm : _fsm = GetComponent<FiniteStateMachine>();
    public DropItem DropItem => _dropItem != null ? _dropItem : _dropItem = GetComponent<DropItem>();
    public StatsSystem StatsSystem => _statsSystem = _statsSystem != null ? _statsSystem : _statsSystem = GetComponent<StatsSystem>();
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
        
    }

    public void SetEnemyData(EnemyAgentDataSO data)
    {
    }

    public Stat GetStat<T>() where T : StatComponentSO
    {
        return StatsSystem.GetStat<T>();
    }

    public Stat GetStat(StatComponentSO statComponent)
    {
        return StatsSystem.GetStat(statComponent);
    }
}
