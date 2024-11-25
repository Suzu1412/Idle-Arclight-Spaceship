using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Cysharp.Text;

public class GeneratorButtonController : MonoBehaviour
{
    [Header("Double Variable")]
    [SerializeField] private DoubleVariableSO _totalCurrency;

    [Header("Void Event Listener")]
    [SerializeField] private VoidGameEventListener OnCurrencyChangedEventListener;
    [SerializeField] private VoidGameEventListener OnGeneratorUpgradeListener;
    [SerializeField] private VoidGameEventListener OnProductionChangedEventListener;

    [Header("Assigned Automatically")]
    [SerializeField][ReadOnly] private GeneratorSO _generator;
    [SerializeField][ReadOnly] private int _index;

    [Header("Button Fields")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _generatorIcon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _productionText;
    [SerializeField] private Button _buyButton;
    private bool _isAlreadyActive;

    public event UnityAction<int> OnBuyGeneratorClicked;

    private void Awake()
    {
        _isAlreadyActive = false;
        ActivateButton(_isAlreadyActive);
    }

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(HandleBuyButton);
        OnCurrencyChangedEventListener.Register(CheckIfCanBuy);
        OnGeneratorUpgradeListener.Register(DisplayImage);
        OnProductionChangedEventListener.Register(DisplayProductionText);

        if (_generator == null) return;

        CheckIfCanBuy();
        DisplayAmountOwned();
        DisplayProductionText();
        DisplayPriceText();
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveAllListeners();
        OnCurrencyChangedEventListener.DeRegister(CheckIfCanBuy);
        OnGeneratorUpgradeListener.DeRegister(DisplayImage);
        OnProductionChangedEventListener.DeRegister(DisplayProductionText);

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
        if (!_isAlreadyActive)
        {
            if (_generator.IsUnlocked || _totalCurrency.Value >= _generator.CostRequirement)
            {
                _isAlreadyActive = true;
                _generator.CheckIfMeetRequirementsToUnlock(_totalCurrency.Value);
                ActivateButton(_isAlreadyActive);
            }
        }

        ToggleBuyButton(_totalCurrency.Value >= _generator.BulkCost);
    }

    private void ToggleBuyButton(bool val)
    {
        if (!val)
        {
            if (EventSystem.current.currentSelectedGameObject == _buyButton.gameObject)
            {
                UIManager.Instance.SetGeneratorShopDefaultButton();
            }
        }

        _buyButton.interactable = val;
    }

    private void ActivateButton(bool val)
    {

        _background.enabled = val;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(val);
        }

    }

    private void DisplayImage()
    {
        _generatorIcon.sprite = _generator.Image;
    }

    private void DisplayAmountOwned()
    {
        _amountText.SetTextFormat("{0}", _generator.AmountOwned);
    }

    private void DisplayName()
    {
        _nameText.SetTextFormat("{0}", _generator.Name);
    }

    private void DisplayPriceText()
    {
        _priceText.SetTextFormat("{0}", _generator.CostFormatted.GetFormat());
    }

    private void DisplayProductionText()
    {
        if (_generator == null) return;
        _productionText.SetTextFormat("{0} CpS", _generator.ProductionFormatted.GetFormat());
    }
}
