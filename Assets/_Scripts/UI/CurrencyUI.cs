using UnityEngine;
using TMPro;
using System.Collections;
using System;
using Cysharp.Text;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class CurrencyUI : MonoBehaviour
{
    private Coroutine _countTo;
    private FormattedNumber _currentValueFormatted;
    private float _currentValue;
    private float _targetValue;

    private BigNumber _startBigNumber;
    private BigNumber _endBigNumber;


    [SerializeField] private CurrencyDataSO _currencyData;

    [SerializeField] private VoidGameEventBinding OnCurrencyChangedEventBinding;
    [SerializeField] private VoidGameEventBinding OnProductionChangedEventBinding;

    [Header("Formatted Number Listener")]
    [SerializeField] private FormattedNumberEventListener OnLoadCurrencyListener;
    [SerializeField] private FormattedNumberEventListener OnUpdateCurrencyTextListener;
    [SerializeField] private FormattedNumberEventListener OnUpdateProductionTextListener;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _currencyText;

    [Header("Localization")]
    [SerializeField] private LocalizeStringEvent _productionLocalized;
    private LocalizedString _localizedString;
    [SerializeField] private string _table = "Tabla1";

    private Coroutine _animateCurrencyCoroutine;

    private StringVariable _amountVariable;
    private Action AnimateCurrencyChangeAction;
    private Action UpdateProductionAction;

    private void Awake()
    {
        _localizedString = _productionLocalized.StringReference;
        AnimateCurrencyChangeAction = AnimateCurrencyChange;
        UpdateProductionAction = UpdateProductionText;
        SetAmountVariable();
    }

    private void OnEnable()
    {
        OnCurrencyChangedEventBinding.Bind(AnimateCurrencyChangeAction, this);
        //OnLoadCurrencyListener.Register(LoadCurrencyText);
        //OnUpdateCurrencyTextListener.Register(UpdateCurrencyText);
        //OnUpdateProductionTextListener.Register(UpdateProductionText);
    }

    private void OnDisable()
    {
        OnCurrencyChangedEventBinding.Unbind(AnimateCurrencyChangeAction, this);
        //OnLoadCurrencyListener.DeRegister(LoadCurrencyText);
        //OnUpdateCurrencyTextListener.DeRegister(UpdateCurrencyText);
        //OnUpdateProductionTextListener.DeRegister(UpdateProductionText);
    }

    private void UpdateCurrencyText(FormattedNumber formatValue)
    {
        if (_countTo != null) StopCoroutine(_countTo);
        _countTo = StartCoroutine(CountToCoroutine(formatValue));
    }

    /// <summary>
    /// Called only on Load Game
    /// </summary>
    /// <param name="formatValue"></param>
    private void LoadCurrencyText(FormattedNumber formatValue)
    {
        _currentValue = formatValue.Value;
        _currentValueFormatted.Init(_currentValue, formatValue.Unit);
        _currencyText.text = _currentValueFormatted.GetFormat();
    }

    private void AnimateCurrencyChange()
    {
        if (_animateCurrencyCoroutine != null) StopCoroutine(_animateCurrencyCoroutine);
        _animateCurrencyCoroutine = StartCoroutine(AnimateCurrencyCoroutine());
    }

    private IEnumerator AnimateCurrencyCoroutine()
    {
        float elapsedTime = 0f;
        float duration = 1f;
        _startBigNumber = _endBigNumber;
        _endBigNumber = _currencyData.TotalCurrency;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            BigNumber lerpedValue = BigNumber.Lerp(_startBigNumber, _endBigNumber, t);

            // Update UI with lerped value formatted as string
            _currencyText.SetTextFormat("{0}", lerpedValue.ToString());

            yield return null;
        }

        // Ensure final value is exactly the end value
        _currencyText.SetTextFormat("{0}", _endBigNumber.ToString());
    }

    private void UpdateProductionText(FormattedNumber formatValue)
    {
        _productionLocalized.StringReference.SetReference(_table, "gpsAmount");
        _amountVariable.Value = formatValue.GetFormat();
    }

    private void UpdateProductionText()
    {

    }

    private IEnumerator CountToCoroutine(FormattedNumber formatValue)
    {
        _targetValue = formatValue.Value;
        var rate = (_targetValue - _currentValue);
        while (_currentValue != _targetValue)
        {
            if (_currentValue < _targetValue)
            {
                _currentValue = Mathf.MoveTowards(_currentValue, _targetValue, rate * Time.deltaTime);
            }
            else
            {
                _currentValue = Mathf.MoveTowards(_currentValue, _targetValue, -rate * 4 * Time.deltaTime);
            }
            _currentValueFormatted.Init(_currentValue, formatValue.Unit);

            _currencyText.SetTextFormat("{0}", _currentValueFormatted.GetFormat());
            yield return null;
        }
    }

    private void SetAmountVariable()
    {
        if (!_localizedString.TryGetValue("amount", out var variable))
        {
            _amountVariable = new StringVariable();
            _localizedString.Add("amount", variable);
        }
        else
        {
            _amountVariable = variable as StringVariable;
        }
    }
}
