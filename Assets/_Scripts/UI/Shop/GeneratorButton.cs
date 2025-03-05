using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using Cysharp.Text;
using UnityEngine.EventSystems;

public class GeneratorButton : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI costText;
    [Header("Generator Info")]
    [SerializeField] private TextMeshProUGUI productionText;
    [SerializeField] private TextMeshProUGUI amountOwnedText;
    [SerializeField] private TextMeshProUGUI _percentageText;
    [SerializeField] private Button buyButton;

    private GeneratorSO generator;
    private CurrencyDataSO _currencyData;

    [Header("Events")]
    [SerializeField] private VoidGameEvent OnProductionChangedEvent;

    [SerializeField] private VoidGameEventBinding OnCurrencyChangedEvent;


    [Header("Data")]
    [SerializeField] private IntVariableSO amountToBuy;

    [Header("Localization")]
    private LocalizedString _localizedString;
    private LocalizedString _productionLocalizedString;
    private LocalizedString _buyLocalizedString;
    [SerializeField] private LocalizeStringEvent _nameLocalized;
    [SerializeField] private LocalizeStringEvent _descriptionLocalized;
    [SerializeField] private LocalizeStringEvent _productionLocalized;
    [SerializeField] private LocalizeStringEvent _buyAmountLocalized;
    [SerializeField] private string _table = "Tabla1";
    private string _gemDescription = "gemDescription";
    private string _gemProduction = "gemProduction";
    private StringVariable _amountVariable;
    private StringVariable _amountProductionVariable;
    private IntVariable _amountToBuyVariable;

    private void Awake()
    {
        _localizedString = _descriptionLocalized.StringReference;
        _productionLocalizedString = _productionLocalized.StringReference;
        _buyLocalizedString = _buyAmountLocalized.StringReference;

        SetAmountVariable();
        SetAmountProductionVariable();
        SetAmountToBuyVariable();
    }

    private void OnEnable()
    {
        OnCurrencyChangedEvent.Bind(ChangeAmountToBuy, this);
    }

    private void OnDisable()
    {
        OnCurrencyChangedEvent.Unbind(ChangeAmountToBuy, this);
    }

    public void Initialize(GeneratorSO generatorData, CurrencyDataSO currencyData)
    {
        generator = generatorData;
        _currencyData = currencyData;
        itemImage.sprite = generator.GetSprite();
        DisplayName();
        UpdateButtonState(_currencyData.TotalCurrency);
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => BuyGenerator());
    }

    public void UpdateButtonState(BigNumber totalCurrency)
    {
        if (generator == null) return;
        costText.text = generator.BulkCost.ToString();
        DisplayProductionText();
        DisplayPercentageText();
        DisplayAmountOwned();
        DisplayDescription();
        ChangeAmountToBuy();
    }

    public void ChangeAmountToBuy()
    {
        if (generator == null) return;

        DisplayAmountToBuy(CalculateAmountToBuy(amountToBuy.Value, _currencyData.TotalCurrency));
        ToggleBuyButton(_currencyData.TotalCurrency >= generator.BulkCost);
        DisplayPriceText();
    }

    private void DisplayName()
    {
        _nameLocalized.StringReference.SetReference(_table, generator.Name);
        _nameLocalized.RefreshString();
    }

    private void DisplayDescription()
    {
        _descriptionLocalized.StringReference.SetReference(_table, _gemDescription);
        _amountVariable.Value = generator.BaseProduction.ToString();
        _descriptionLocalized.RefreshString();
    }

    private void DisplayProductionText()
    {
        _productionLocalized.StringReference.SetReference(_table, _gemProduction);
        _amountProductionVariable.Value = generator.Production.ToString();
        _productionLocalized.RefreshString();
    }

    private void DisplayPercentageText()
    {
        _percentageText.SetTextFormat("{0}%", generator.ProductionPercentage.ToString("F2"));
    }

    private void DisplayAmountOwned()
    {
        amountOwnedText.SetTextFormat("{0}", generator.AmountOwned);
    }

    private void DisplayAmountToBuy(int amount)
    {
        _amountToBuyVariable.Value = amount;
        _buyAmountLocalized.RefreshString();
    }

    private void DisplayPriceText()
    {
        costText.SetTextFormat("{0}", generator.BulkCost.ToString());
    }

    private int CalculateAmountToBuy(int amount, BigNumber totalCurrency)
    {
        int currentAmount = (amount > 0) ? amount : generator.GetMaxGenerators(totalCurrency);

        // Ensure we don't use an invalid or infinite number
        if (currentAmount <= 0) currentAmount = 1;

        // Store the total cost instead of just calling GetBulkCost
        BigNumber bulkCost = generator.GetBulkCost(currentAmount);

        return currentAmount;
    }

    private void BuyGenerator()
    {
        if (_currencyData.TotalCurrency >= generator.BulkCost)
        {
            _currencyData.SubtractCurrency(generator.BulkCost);
            generator.AddAmount(_amountToBuyVariable.Value);
            generator.CalculateProductionRate();
            UpdateButtonState(_currencyData.TotalCurrency);
            ChangeAmountToBuy();
            OnProductionChangedEvent.RaiseEvent(this);
        }
    }

    private void ToggleBuyButton(bool val)
    {
        buyButton.interactable = val;
    }

    private void SetAmountVariable()
    {
        if (!_localizedString.TryGetValue("amount", out var variable))
        {
            _amountVariable = new StringVariable();
            _localizedString.Add("amount", _amountVariable);
        }
        else
        {
            _amountVariable = variable as StringVariable;
        }
    }

    private void SetAmountToBuyVariable()
    {
        if (!_buyLocalizedString.TryGetValue("amountToBuy", out var variable))
        {
            _amountToBuyVariable = new IntVariable();
            _buyLocalizedString.Add("amount", _amountToBuyVariable);
        }
        else
        {
            _amountToBuyVariable = variable as IntVariable;
        }
    }

    private void SetAmountProductionVariable()
    {
        if (!_productionLocalizedString.TryGetValue("amount", out var variable))
        {
            _amountProductionVariable = new StringVariable();
            _productionLocalizedString.Add("amount", _amountProductionVariable);
        }
        else
        {
            _amountProductionVariable = variable as StringVariable;
        }
    }
}
