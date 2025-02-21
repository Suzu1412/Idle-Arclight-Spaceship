using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorButton : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI productionText;
    [SerializeField] private Button buyButton;

    private GeneratorSO generator;
    private CurrencyDataSO _currencyData;

    public void Initialize(GeneratorSO generatorData, CurrencyDataSO currencyData)
    {
        if (costText == null)
        {
            Debug.Log("costo vacio");
        }
        if (productionText == null)
        {
            Debug.Log("production vacio");
        }
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
        productionText.text = generator.Production.ToString();
        buyButton.interactable = totalCurrency >= generator.BulkCost;
    }

    private void BuyGenerator()
    {
        if (_currencyData.TotalCurrency >= generator.BulkCost)
        {
            _currencyData.SubtractCurrency(generator.BulkCost);
            UpdateButtonState(_currencyData.TotalCurrency);
        }
    }
}
