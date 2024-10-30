using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class CurrencyUI : MonoBehaviour
{
    private Coroutine _countTo;
    private float _currentValue;
    private float _targetValue;
    [Header("Formatted Numbe Listener")]
    [SerializeField] private FormattedNumberEventListener OnUpdateCurrencyTextListener;
    [SerializeField] private FormattedNumberEventListener OnUpdateProductionTextListener;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _currencyText;
    [SerializeField] private TextMeshProUGUI _productionText;

    private void OnEnable()
    {
        OnUpdateCurrencyTextListener.Register(UpdateCurrencyText);
        OnUpdateProductionTextListener.Register(UpdateProductionText);
    }

    private void OnDisable()
    {
        OnUpdateCurrencyTextListener.DeRegister(UpdateCurrencyText);
        OnUpdateProductionTextListener.DeRegister(UpdateProductionText);
    }

    private void UpdateCurrencyText(FormattedNumber formatValue)
    {
        if (_countTo != null) StopCoroutine(_countTo);
        _countTo = StartCoroutine(CountToCoroutine(formatValue));
    }

    private void UpdateProductionText(FormattedNumber formatValue)
    {
        _productionText.text = formatValue.GetFormat() + " CPS";
    }

    private IEnumerator CountToCoroutine(FormattedNumber formatValue)
    {
        _targetValue = (float)formatValue.Value;
        var rate = (_targetValue - _currentValue);
        while (_currentValue != _targetValue)
        {
            _currentValue = Mathf.MoveTowards(_currentValue, _targetValue, rate * Time.deltaTime);
            _currencyText.text = _currentValue.ToString("0.##") + formatValue.Unit;
            yield return null;
        }
        _currentValue = _targetValue;
    }
}
