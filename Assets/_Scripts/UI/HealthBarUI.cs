using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Text;

public class HealthBarUI : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Image _damagebar;
    [SerializeField] private Image _healthBar;
    [SerializeField] private IntVariableSO _health;
    [SerializeField] private VoidGameEventListener OnHealthChangedEventListener;
    [SerializeField] private IntGameEventListener OnDamagedEventListener;
    [SerializeField] private IntGameEventListener OnHealedEventListener;
    private float _previousValue;
    [SerializeReference] private IHealthSystem _healthSystem;
    private Coroutine _damageAnimationCoroutine;
    private Coroutine _healAnimationCoroutine;
    private bool _isDamaged;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private float _damageAnimationDelay = 0.2f;
    [SerializeField] private float _damageAnimationDuration = 0.01f;

    private void OnEnable()
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnMaxHealthValueChanged += SetMaxHealth;
            _healthSystem.OnDamaged += ReceiveDamage;
            _healthSystem.OnHealed += ReceiveHeal;

        }

        OnHealthChangedEventListener?.Register(UpdateHealth);
        OnDamagedEventListener?.Register(ReceiveDamage);
        OnHealedEventListener?.Register(ReceiveHeal);
    }

    private void OnDisable()
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnMaxHealthValueChanged -= SetMaxHealth;
            _healthSystem.OnDamaged -= ReceiveDamage;
            _healthSystem.OnHealed -= ReceiveHeal;
        }

        OnHealthChangedEventListener?.DeRegister(UpdateHealth);
        OnDamagedEventListener?.DeRegister(ReceiveDamage);
        OnHealedEventListener?.DeRegister(ReceiveHeal);

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

    private void ReceiveHeal(int amount)
    {
        UpdateText();
        if (_healAnimationCoroutine != null) StopCoroutine(_healAnimationCoroutine);
        _healAnimationCoroutine = StartCoroutine(HealAnimationCoroutine());
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

        _damagebar.fillAmount = _healthBar.fillAmount;
        _healthBar.fillAmount = _health.Ratio;
        yield return Helpers.GetWaitForSeconds(_damageAnimationDelay);

        float elapsedTime = 0f;
        float startValue = _damagebar.fillAmount;

        while (elapsedTime < _damageAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _damageAnimationDuration; // Normalize elapsed time
            _damagebar.fillAmount = Mathf.Lerp(startValue, _health.Ratio, t);
            yield return null;
        }

        // Ensure the final value is set correctly
        _damagebar.fillAmount = 0f;
        _isDamaged = false;

    }

    IEnumerator HealAnimationCoroutine()
    {
        float elapsedTime = 0f;
        float startValue = _healthBar.fillAmount;

        while (elapsedTime < _damageAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _damageAnimationDuration; // Normalize elapsed time
            _healthBar.fillAmount = Mathf.Lerp(startValue, _health.Ratio, t);
            yield return null;
        }

        yield return null;


        _healthBar.fillAmount = _health.Ratio;
    }
}
