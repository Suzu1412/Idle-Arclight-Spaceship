using UnityEngine;
using TMPro;
using System.Collections;
using System;
using Cysharp.Text;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
    private BigNumber _startBigNumber;
    private BigNumber _endBigNumber;

    [SerializeField] private CurrencyDataSO _currencyData;

    [SerializeField] private VoidGameEventBinding OnCurrencyChangedEventBinding;
    [SerializeField] private VoidGameEventBinding OnProductionChangedEventBinding;

    [SerializeField] private RectTransform _productionRectTransform;

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
        OnProductionChangedEventBinding.Bind(UpdateProductionAction, this);
    }

    private void OnDisable()
    {
        OnCurrencyChangedEventBinding.Unbind(AnimateCurrencyChangeAction, this);
        OnProductionChangedEventBinding.Unbind(UpdateProductionAction, this);
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

    private void UpdateProductionText()
    {
        UIAnimationManager.Instance.ScalePop(_productionRectTransform, 1.3f, 0.4f, 1f);
        _productionLocalized.StringReference.SetReference(_table, "gpsAmount");
        _amountVariable.Value = _currencyData.TotalProduction.ToString();
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
