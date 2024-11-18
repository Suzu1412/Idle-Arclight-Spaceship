using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsSystem))]
public class HealthSystem : MonoBehaviour, IHealthSystem
{
    [SerializeField] private IntVariableSO _health;

    private IAgent _agent;
    private StatType _statType;
    private bool _isHurt;
    private bool _isDeath;
    private bool _isInvulnerable;
    private int _knockbackDirection;
    [SerializeField] private float _hurtDuration = 0.2f;
    [SerializeField] private float _defaultInvulnerability = 0.1f;
    private float _invulnerabilityDuration;
    private Coroutine _hurtPeriodCoroutine;
    private Coroutine _invulnerabilityPeriodCoroutine;

    internal IAgent Agent => _agent ??= _agent = GetComponent<IAgent>();

    public bool IsHurt => _isHurt;
    public bool IsDeath => _isDeath;
    public int KnockbackDirection => _knockbackDirection;
    public float HurtDuration => _hurtDuration;
    public bool IsInvulnerable => _isInvulnerable;
    public float InvulnerabilityDuration => _invulnerabilityDuration;

    [SerializeField] private VoidGameEvent OnHealthChangedEvent;
    [SerializeField] private IntGameEvent OnDamagedEvent;

    #region Events
    public event Action<IntVariableSO> OnMaxHealthValueChanged;
    public event Action<IntVariableSO> OnHealthValueChanged;
    public event Action<int> OnHealed;
    public event Action<int> OnDamaged;
    public event Action OnDeath;
    public event Action OnHitStun;
    public event Action OnHit;
    public event Action OnInvulnerabilityPeriod;
    #endregion

    private void Awake()
    {
        _statType = StatType.MaxHealth;
    }

    public void Initialize(int currentHealth)
    {
        if (_health == null) _health = ScriptableObject.CreateInstance<IntVariableSO>();
        if (currentHealth == 0)
        {
            _health.Initialize(GetMaxValue(), 0, GetMaxValue());
        }
        else
        {
            _health.Initialize(currentHealth, 0, GetMaxValue());
        }

        OnMaxHealthValueChanged?.Invoke(_health);
        OnHealthValueChanged?.Invoke(_health);
        OnHealthChangedEvent?.RaiseEvent();
    }

    private void OnEnable()
    {
        Agent.StatsSystem.OnMaxHealthChange += UpdateMaxHealth;
        _isDeath = false;
        _isHurt = false;
    }

    private void OnDisable()
    {
        Agent.StatsSystem.OnMaxHealthChange -= UpdateMaxHealth;
        _isInvulnerable = false;
    }

    private int GetMinPossiblevalue()
    {
        return Mathf.RoundToInt(Agent.GetStatMinValue(_statType));
    }

    private int GetMaxPossibleValue()
    {
        return Mathf.RoundToInt(Agent.GetStatMaxValue(_statType));
    }

    private int GetMaxValue()
    {
        return Mathf.RoundToInt(Agent.GetStat(_statType));
    }

    public int GetMaxHealth()
    {
        return _health.MaxValue;
    }

    public int GetCurrentHealth()
    {
        return _health.Value;
    }

    private void UpdateMaxHealth()
    {
        _health.MaxValue = GetMaxValue();
        _health.Value = _health.MaxValue;
        OnHealthValueChanged?.Invoke(_health);
        OnHealthChangedEvent?.RaiseEvent();
    }

    public void Heal(int amount)
    {
        if (amount <= 0f) return;

        _health.Value += amount;
        OnHealed?.Invoke(amount);
    }

    public void Damage(int amount)
    {
        if (amount <= 0f || _isInvulnerable) return;

        _health.Value -= amount;
        OnHit?.Invoke();
        OnDamaged?.Invoke(amount);
        OnDamagedEvent?.RaiseEvent(amount);


        if (_health.Value <= 0)
        {
            Death();
            return;
        }

        SetInvulnerability(true, _defaultInvulnerability);
    }

    public void Death(bool invokeEvents = true)
    {
        if (invokeEvents)
        {
            OnDeath?.Invoke();
        }

        transform.parent.gameObject.SetActive(false);
    }

    public void GetHit(GameObject damageDealer)
    {
        if (_isInvulnerable) return;

        OnHitStun?.Invoke();
    }

    public void SetInvulnerability(bool isInvulnerable, float duration)
    {
        _isInvulnerable = isInvulnerable;
        duration = Mathf.Clamp(duration, 0f, 5f);
        if (duration == 0) return;
        _invulnerabilityDuration = duration;
        OnInvulnerabilityPeriod?.Invoke();

        if (_invulnerabilityPeriodCoroutine != null) StopCoroutine(_invulnerabilityPeriodCoroutine);
        _invulnerabilityPeriodCoroutine = StartCoroutine(InvulnerabilityPeriodCoroutine());
    }

    private IEnumerator InvulnerabilityPeriodCoroutine()
    {
        _isInvulnerable = true;
        yield return Helpers.GetWaitForSeconds(_invulnerabilityDuration);
        _isInvulnerable = false;
    }

    public float GetHealthPercent()
    {
        return _health.Ratio;
    }
}
