using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsSystem))]
public class HealthSystem : MonoBehaviour, IHealthSystem
{
    [SerializeField] private IntVariableSO _health;

    private IAgent _agent;
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
    [SerializeField] private IntGameEvent OnHealedEvent;

    #region Events
    public event Action<IntVariableSO> OnMaxHealthValueChanged;
    public event Action OnHealthValueChanged;
    public event Action<int> OnHealed;
    public event Action<int> OnDamaged;
    public event Action OnDeath;
    public event Action OnHitStun;
    public event Action OnHit;
    public event Action OnInvulnerabilityPeriod;
    public event Action OnHeal;
    public event Action OnRemove;
    #endregion


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
        OnHealthValueChanged?.Invoke();
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
        return Mathf.RoundToInt(Agent.StatsSystem.GetStatMinValue<HealthStatSO>());
    }

    private int GetMaxPossibleValue()
    {
        return Mathf.RoundToInt(Agent.StatsSystem.GetStatMaxValue<HealthStatSO>());
    }

    private int GetMaxValue()
    {
        return Mathf.RoundToInt(Agent.StatsSystem.GetStatValue<HealthStatSO>());
    }

    public int GetMaxHealth()
    {
        return _health.MaxValue;
    }

    public int GetCurrentHealth()
    {
        return _health != null ? _health.Value : 1;
    }

    private void UpdateMaxHealth()
    {
        _health.MaxValue = GetMaxValue();
        _health.Value = _health.MaxValue;
        OnHealthValueChanged?.Invoke();
        OnHealthChangedEvent?.RaiseEvent();
    }

    public void Heal(int amount)
    {
        if (amount <= 0f) return;

        _health.Value += amount;
        OnHeal?.Invoke();
        OnHealed?.Invoke(amount);
        OnHealedEvent?.RaiseEvent(amount);
        OnHealthValueChanged?.Invoke();
    }

    public void Damage(int amount, bool ignoreInvulnerability = false)
    {
        if (amount <= 0f) return;
        if (!ignoreInvulnerability && _isInvulnerable) return;

        if (_health == null)
        {
            Initialize(0);
        }
        _health.Value -= amount;
        OnHit?.Invoke();
        OnDamaged?.Invoke(amount);
        OnDamagedEvent?.RaiseEvent(amount);
        OnHealthValueChanged?.Invoke();

        SetInvulnerability(isInvulnerable: true, _defaultInvulnerability);

        if (_health.Value <= 0f)
        {
            Death(gameObject, DeathCauseType.EnemyAttack);
            return;
        }
    }

    public void Death(GameObject source, DeathCauseType cause)
    {
        if (_isDeath)
        {
            Debug.Log($"{gameObject.transform.parent} is already death");
            return;
        }

        if (!IsValidSource(source))
        {
            return;
        }

        _isDeath = true;
        HandleDeathCause(cause);
    }


    public void GetHit(GameObject damageDealer)
    {
        if (_isInvulnerable) return;

        OnHitStun?.Invoke();
    }

    public async void SetInvulnerability(bool isInvulnerable, float duration)
    {
        _isInvulnerable = isInvulnerable;
        duration = Mathf.Clamp(duration, 0f, 5f);
        if (duration == 0) return;
        _invulnerabilityDuration = duration;
        OnInvulnerabilityPeriod?.Invoke();

        await Awaitable.WaitForSecondsAsync(duration);
        _isInvulnerable = false;
    }

    public float GetHealthPercent()
    {
        return _health.Ratio;
    }

    private bool IsValidSource(GameObject source)
    {
        return source == gameObject || source.CompareTag("DeadZone");
    }
    private void HandleDeathCause(DeathCauseType cause)
    {
        switch (cause)
        {
            case DeathCauseType.EnemyAttack:
                OnDeath?.Invoke();
                break;

            case DeathCauseType.DeadZone:
                // Doesn't give any reward
                break;

            case DeathCauseType.Kamikaze:
                // Doesn't give any reward
                break;

            case DeathCauseType.Instakill:
                OnDeath?.Invoke();
                break;

            default:
                Debug.LogError("Death cause not set for " + this.gameObject);
                break;
        }
    }

    private void InvokeDeathEvents()
    {
        OnDeath?.Invoke();
    }

    public void Remove(GameObject source)
    {
        if (!_isDeath)
        {
            Debug.LogError("Must Call Death Method Before removing character");
            return;
        }

        if (!IsValidSource(source))
        {
            return;
        }

        OnRemove?.Invoke();
    }
}
