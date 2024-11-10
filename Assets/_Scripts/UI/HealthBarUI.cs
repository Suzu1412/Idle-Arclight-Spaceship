using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image _damagebar;
    [SerializeField] private Image _healthBar;
    [SerializeField] private IntVariableSO _health;
    [SerializeField] private VoidGameEventListener OnHealthChangedEventListener;
    [SerializeField] private IntGameEventListener OnDamagedEventListener;
    private float _previousValue;
    private IHealthSystem _healthSystem;
    private Coroutine _damageAnimationCoroutine;
    [SerializeField] private float _lerpDuration = 1f;
    private bool _isDamaged;
    [SerializeField] private TextMeshProUGUI _amountText;

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
            _healthSystem.OnDamaged -= ReceiveDamage;
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
        SetHealthBar();
    }

    public void SetHealth(IntVariableSO health)
    {
        _health = health;
    }

    private void UpdateHealth()
    {
        Debug.Log($"{_health.Value} - {_health.MaxValue}");
        SetHealthBar();
    }

    private void UpdateText()
    {
        if (_amountText == null) return;
        _amountText.text = (_health.Value + " / " + _health.MaxValue).ToString();
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
        yield return Helpers.GetWaitForSeconds(0.5f);
        _damagebar.DOFillAmount(_health.Ratio, 0.5f);
        _isDamaged = false;

    }
}
