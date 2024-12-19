using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cysharp.Text;

public class HealthBarUI : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Image _damagebar;
    [SerializeField] private Image _healthBar;
    [SerializeField] private IntVariableSO _health;
    [SerializeField] private VoidGameEventListener OnHealthChangedEventListener;
    [SerializeField] private IntGameEventListener OnDamagedEventListener;
    private float _previousValue;
    private IHealthSystem _healthSystem;
    private Coroutine _damageAnimationCoroutine;
    private bool _isDamaged;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private float _damageAnimationDelay = 0.2f;
    [SerializeField] private float _damageAnimationDuration = 0.01f;

    private void Awake()
    {
        _healthSystem = GetComponentInParent<IHealthSystem>();
    }

    private void OnEnable()
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnMaxHealthValueChanged += SetMaxHealth;
            _healthSystem.OnHealthValueChanged += SetHealth;
            _healthSystem.OnDamaged += ReceiveDamage;

        }

        OnHealthChangedEventListener?.Register(UpdateHealth);
        OnDamagedEventListener?.Register(ReceiveDamage);
    }

    private void OnDisable()
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnMaxHealthValueChanged -= SetMaxHealth;
            _healthSystem.OnHealthValueChanged -= SetHealth;
            _healthSystem.OnDamaged -= ReceiveDamage;
        }

        OnHealthChangedEventListener?.DeRegister(UpdateHealth);
        OnDamagedEventListener?.DeRegister(ReceiveDamage);
    }

    private void Start()
    {
        if (_health == null) return;
        UpdateHealth();
    }

    public void SetMaxHealth(IntVariableSO health)
    {
        _health = health;
        _isDamaged = false;
        if (_health.Value > 0)
        {
            SetHealthBar();
        }
    }

    public void SetHealth(IntVariableSO health)
    {
        _health = health;
    }

    private void UpdateHealth()
    {
        if (_health.Value > 0)
        {
            SetHealthBar();
        }
    }

    private void UpdateText()
    {
        if (_amountText == null) return;
        _amountText.SetTextFormat("{0} / {1}", _health.Value, +_health.MaxValue);
    }

    private void SetHealthBar()
    {
        if (FloatComparison.TolerantLesserThanOrEquals(_health.MaxValue, 0))
        {
            // if Max health is zero, ratio will be infinity (divide by zero)
            // we just return 1.0 here for safety
            _damagebar.fillAmount = 1f;
            _previousValue = _damagebar.fillAmount;
            UpdateText();
            Debug.LogError($"Max Health set to 0. Correct Data values in: ", this.gameObject);
        }
        else
        {
            _damagebar.fillAmount = 0f;
            _healthBar.fillAmount = _health.Ratio;
            UpdateText();
        }
    }

    private void ReceiveDamage(int amount)
    {
        UpdateText();
        if (_damageAnimationCoroutine != null) StopCoroutine(_damageAnimationCoroutine);
        _damageAnimationCoroutine = StartCoroutine(DamageAnimationCoroutine());
    }

    IEnumerator DamageAnimationCoroutine()
    {
        if (!_isDamaged)
        {
            _damagebar.fillAmount = _healthBar.fillAmount;
            _isDamaged = true;
        }

        //_damagebar.fillAmount = _healthBar.fillAmount;
        _healthBar.fillAmount = _health.Ratio;
        yield return Helpers.GetWaitForSeconds(_damageAnimationDelay);
        _damagebar.DOFillAmount(_health.Ratio, _damageAnimationDuration);
        _isDamaged = false;

    }
}
