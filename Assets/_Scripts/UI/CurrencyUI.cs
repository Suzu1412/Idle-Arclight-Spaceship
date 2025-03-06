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
    private BigNumber _startBigNumber = new BigNumber(0);
    private BigNumber _endBigNumber;
    private BigNumber _lerpedNumber;

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

    [Header("Currency Animation")]
    [SerializeField] private float velocity = 0;
    [SerializeField] private float smoothTime = 0.2f;

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
        _endBigNumber = _currencyData.TotalCurrency; // Just update the end value

        if (_animateCurrencyCoroutine == null)
        {
            _animateCurrencyCoroutine = StartCoroutine(AnimateCurrencyCoroutine());
        }
    }

    private IEnumerator AnimateCurrencyCoroutine()
    {
        float mantissaVelocity = 0f;
        float exponentVelocity = 0f;
        float elapsedTime = 0f;


        while (true)
        {
 

            _lerpedNumber = BigNumber.SmoothDamp(_lerpedNumber, _currencyData.TotalCurrency, ref mantissaVelocity, ref exponentVelocity, smoothTime);
            Debug.Log($"lerped: {_lerpedNumber.exponent}");

            _currencyText.SetTextFormat("{0}", _lerpedNumber.ToString());

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
