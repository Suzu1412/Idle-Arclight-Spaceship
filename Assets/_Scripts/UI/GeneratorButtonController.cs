using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GeneratorButtonController : MonoBehaviour
{
    [Header("Double Variable")]
    [SerializeField] private DoubleVariableSO _totalCurrency;

    [Header("Void Event Listener")]
    [SerializeField] private VoidGameEventListener OnCurrencyChangedEventListener;

    [Header("Assigned Automatically")]
    [SerializeField] [ReadOnly] private GeneratorSO _generator;
    [SerializeField] [ReadOnly] private int _index;

    [Header("Button Fields")]
    [SerializeField] private Image _generatorIcon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private TextMeshProUGUI _buyText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _productionText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Sprite _buttonAvailableSelectedSprite;

    public double Cost => _generator.Cost;

    public event UnityAction<int> OnBuyGeneratorClicked;

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(HandleBuyButton);
        OnCurrencyChangedEventListener.Register(CheckIfCanBuy);
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveAllListeners();
        OnCurrencyChangedEventListener.DeRegister(CheckIfCanBuy);
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void SetGenerator(GeneratorSO generator)
    {
        _generator = generator;
    }

    public void PrepareButton()
    {
        DisplayImage();
        DisplayAmountOwned();
        DisplayName();
        DisplayPriceText();
        DisplayProductionText();
    }

    public void ChangeAmountToBuy(int amount)
    {
        _generator.GetBulkCost(amount);
        CheckIfCanBuy();
        DisplayPriceText();
    }

    public void HandleBuyButton()
    {
        OnBuyGeneratorClicked?.Invoke(_index);
    }

    private void CheckIfCanBuy()
    {
        ToggleBuyButton(_totalCurrency.Value >= _generator.Cost);
    }

    private void ToggleBuyButton(bool val)
    {
        if (!val)
        {
            if (EventSystem.current.currentSelectedGameObject == _buyButton.gameObject)
            {
                UIManager.Instance.SetShopDefaultButton();
            }
        }

        _buyButton.interactable = val;
    }

    private void ActivateButton(bool val)
    {
        if (val)
        {

        }
        else
        {

        }
    }

    private void DisplayImage()
    {
        _generatorIcon.sprite = _generator.Image;
    }

    private void DisplayAmountOwned()
    {
        _amountText.text = _generator.AmountOwned.ToString();
    }

    private void DisplayName()
    {
        _nameText.text = _generator.Name.ToString();
    }

    private void DisplayPriceText()
    {
        _priceText.text = _generator.CostFormatted.GetFormat();
    }

    private void DisplayProductionText()
    {
        _productionText.text = $"{_generator.ProductionFormatted.GetFormat()} CPS";
    }
}
