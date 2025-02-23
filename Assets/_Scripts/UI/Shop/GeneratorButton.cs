using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GeneratorButton : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI costText;
    [Header("Generator Info")]
    [SerializeField] private TextMeshProUGUI productionText;
    [SerializeField] private TextMeshProUGUI amountOwnedText;
    [SerializeField] private Button buyButton;

    private GeneratorSO generator;
    private CurrencyDataSO _currencyData;

    [Header("Events")]
    [SerializeField] private VoidGameEvent OnProductionChangedEvent;

    public void Initialize(GeneratorSO generatorData, CurrencyDataSO currencyData)
    {
        generator = generatorData;
        _currencyData = currencyData;
        itemImage.sprite = generator.GetSprite();
        UpdateButtonState(_currencyData.TotalCurrency);
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => BuyGenerator());
    }

    public void UpdateButtonState(BigNumber totalCurrency)
    {
        if (generator == null) return;
        costText.text = generator.BulkCost.ToString();
        UpdateProduction();
        buyButton.interactable = totalCurrency >= generator.BulkCost;
    }

    private void UpdateProduction()
    {
        productionText.text = generator.Production.ToString();
        amountOwnedText.text = generator.AmountOwned.ToString();
    }

    private void BuyGenerator()
    {
        if (_currencyData.TotalCurrency >= generator.BulkCost)
        {
            _currencyData.SubtractCurrency(generator.BulkCost);
            generator.AddAmount(1);
            generator.CalculateProductionRate();
            UpdateButtonState(_currencyData.TotalCurrency);
            OnProductionChangedEvent.RaiseEvent(this);
        }
    }
}
