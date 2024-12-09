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

    private StringVariable _amountVariable;

    private Utf16ValueStringBuilder _sb;

    private void Awake()
    {
        _localizedString = _productionLocalized.StringReference;
        _sb = ZString.CreateStringBuilder();
        SetAmountVariable();
    }

    private void OnEnable()
    {
        OnLoadCurrencyListener.Register(LoadCurrencyText);
        OnUpdateCurrencyTextListener.Register(UpdateCurrencyText);
        OnUpdateProductionTextListener.Register(UpdateProductionText);
    }

    private void OnDisable()
    {
        OnLoadCurrencyListener.DeRegister(LoadCurrencyText);
        OnUpdateCurrencyTextListener.DeRegister(UpdateCurrencyText);
        OnUpdateProductionTextListener.DeRegister(UpdateProductionText);
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

    private void UpdateProductionText(FormattedNumber formatValue)
    {
        _productionLocalized.StringReference.SetReference(_table, "gpsAmount");
        _amountVariable.Value = formatValue.GetFormat();
        //_productionText.SetTextFormat("{0} CpS", formatValue.GetFormat());
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

            _sb.Clear();
            _currentValueFormatted.GetFormat();
            //_sb.AppendFormat("{0}", );

            //_currencyText.SetText(_sb.ToString());
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
