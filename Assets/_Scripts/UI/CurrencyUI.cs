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
    private BigNumber _currentNumber;
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
    [SerializeField] private UIAnimationManager _animationManager;

    private StringVariable _amountVariable;
    private Action AnimateCurrencyChangeAction;
    private Action UpdateProductionAction;

    private void Awake()
    {
        if (_animationManager == null) _animationManager = FindAnyObjectByType<UIAnimationManager>();
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
        _productionRectTransform.localScale = Vector3.one;
    }

    private void AnimateCurrencyChange()
    {
        if (_animateCurrencyCoroutine == null)
        {
            _animateCurrencyCoroutine = StartCoroutine(AnimateCurrencyCoroutine());
        }
    }

    private IEnumerator AnimateCurrencyCoroutine()
    {
        const float lerpDuration = 1f; // 1 second for smooth transition

        while (true)
        {
            // Get mantissa and exponent of both numbers
            double currentMantissa = _currentNumber.mantissa;
            double targetMantissa = _currencyData.TotalCurrency.mantissa;
            int currentExponent = _currentNumber.exponent;
            int targetExponent = _currencyData.TotalCurrency.exponent;

            // Smoothly adjust exponent if needed
            if (currentExponent != targetExponent)
            {
                // Move exponent towards target
                int exponentDiff = targetExponent - currentExponent;
                int step = Math.Sign(exponentDiff); // +1 or -1
                currentExponent += step;

                // Adjust mantissa to maintain scale
                currentMantissa /= (step > 0) ? 10 : 0.1;
            }
            float elapsedTime = 0f;

            while (elapsedTime < lerpDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / lerpDuration); // Normalize t between 0 and 1

                // Lerp only the mantissa, update exponent smoothly
                double lerpedMantissa = Mathf.Lerp((float)currentMantissa, (float)targetMantissa, t);
                int lerpedExponent = (int)Mathf.Lerp(currentExponent, targetExponent, t);

                _currentNumber = new BigNumber(lerpedMantissa, lerpedExponent);
                //_lerpedNumber = new BigNumber(lerpedMantissa, currentExponent);

                // Update UI
                _currencyText.SetTextFormat("{0}", _currentNumber.GetFormat());

                yield return null;
            }
            // Update UI text
            _currencyText.SetTextFormat("{0}", _currentNumber.GetFormat());

            yield return null;
        }
    }

    private void UpdateProductionText()
    {
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
