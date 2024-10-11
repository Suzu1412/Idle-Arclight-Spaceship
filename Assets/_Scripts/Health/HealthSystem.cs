using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsSystem))]
public class HealthSystem : MonoBehaviour, IHealthSystem
{
    [SerializeField] private HealthSO _health;

    private IAgent _agent;
    private StatType _statType;
    private bool _isHurt;
    private bool _isDeath;
    private bool _isInvulnerable;
    private int _knockbackDirection;
    [SerializeField] private float _hurtDuration = 0.2f;
    [SerializeField] private float _invulnerabilityDuration = 1f;
    [SerializeField] private GameObject _floatingText;
    private Coroutine _hurtPeriodCoroutine;
    private Coroutine _invulnerabilityPeriodCoroutine;

    internal IAgent Agent => _agent ??= _agent = GetComponent<IAgent>();

    public bool IsHurt => _isHurt;
    public bool IsDeath => _isDeath;
    public int KnockbackDirection => _knockbackDirection;
    public float HurtDuration => _hurtDuration;
    public float InvulnerabilityDuration => _invulnerabilityDuration;

    #region Events
    public event Action<float, float> OnMaxHealthValueChanged;
    public event Action<float, float> OnHealthValueChanged;
    public event Action OnHealed;
    public event Action OnDamaged;
    public event Action OnDeath;
    public event Action OnHitStun;
    #endregion

    private void Awake()
    {
        _statType = StatType.MaxHealth;
        Initialize();
    }

    internal void Initialize()
    {
        if (_health == null) _health = ScriptableObject.CreateInstance<HealthSO>();

        _health.Initialize(GetMaxValue(), GetMinPossiblevalue(), GetMaxPossibleValue());
        OnMaxHealthValueChanged?.Invoke(_health.CurrentValue, _health.MaxValue);
    }

    private void OnEnable()
    {
        Agent.StatsSystem.OnMaxHealthChange += UpdateMaxHealth;
    }

    private void OnDisable()
    {
        Agent.StatsSystem.OnMaxHealthChange -= UpdateMaxHealth;
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
        return _health.CurrentValue;
    }

    private void UpdateMaxHealth()
    {
        _health.MaxValue = GetMaxValue();
        _health.CurrentValue = _health.MaxValue;
        OnMaxHealthValueChanged?.Invoke(_health.CurrentValue, _health.MaxValue);
    }

    public void Heal(int amount)
    {
        if (amount <= 0f) return;

        _health.CurrentValue += amount;
        OnHealthValueChanged?.Invoke(_health.CurrentValue, _health.MaxValue);
        OnHealed?.Invoke();
    }

    public void Damage(int amount)
    {
        if (amount <= 0f || _isInvulnerable) return;

        _health.CurrentValue -= amount;
        OnHealthValueChanged?.Invoke(_health.CurrentValue, _health.MaxValue);
        OnDamaged?.Invoke();

        Debug.Log(_health.CurrentValue);

        if (_health.CurrentValue <= 0)
        {
            Death();
            return;
        }

        if (_hurtPeriodCoroutine != null) StopCoroutine(_hurtPeriodCoroutine);
        _hurtPeriodCoroutine = StartCoroutine(HurtPeriodCoroutine());

        if (_invulnerabilityPeriodCoroutine != null) StopCoroutine(_invulnerabilityPeriodCoroutine);
        _invulnerabilityPeriodCoroutine = StartCoroutine(InvulnerabilityPeriodCoroutine());
    }

    public void Death()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }

    public void GetHit(GameObject damageDealer)
    {
        if (_isInvulnerable) return;

        if (transform.position.x < damageDealer.transform.position.x)
        {
            _knockbackDirection = -1;
        }
        else
        {
            _knockbackDirection = 1;
        }
        OnHitStun?.Invoke();
    }

    public void SetInvulnerability(bool isInvulnerable)
    {
        _isInvulnerable = isInvulnerable;
    }

    private IEnumerator HurtPeriodCoroutine()
    {
        _isHurt = true;
        yield return Helpers.GetWaitForSeconds(_hurtDuration);
        _isHurt = false;
    }

    private IEnumerator InvulnerabilityPeriodCoroutine()
    {
        _isInvulnerable = true;
        yield return Helpers.GetWaitForSeconds(_invulnerabilityDuration);
        _isInvulnerable = false;
    }
}
