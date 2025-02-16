using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Cysharp.Text;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using System;
using System.Reflection;

public class GeneratorButtonController : MonoBehaviour
{
    [Header("Run Time Set")]
    [SerializeField] private GameObjectRuntimeSetSO _generatorRTS;

    [Header("Int Variable")]
    [SerializeField] private IntVariableSO _amountToBuy;
    [Header("Double Variable")]
    [SerializeField] private DoubleVariableSO _totalCurrency;

    [Header("Void Event Binding")]
    [SerializeField] private VoidGameEventBinding OnCurrencyChangedEventBinding;
    [SerializeField] private VoidGameEventBinding OnProductionChangedEventBinding;

    [Header("Assigned Automatically")]
    //[SerializeField][ReadOnly] private GeneratorSO _generator;
    [SerializeField][ReadOnly] private int _index;

    [Header("Button Fields")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _generatorIcon;
    [SerializeField] private LocalizeStringEvent _nameLocalized;
    [SerializeField] private LocalizeStringEvent _descriptionLocalized;
    [SerializeField] private LocalizeStringEvent _productionLocalized;
    [SerializeField] private LocalizeStringEvent _buyAmountLocalized;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _percentageText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Image _notificationImage;

    [Header("Notification Icons")]
    [SerializeField] private Sprite _notificationOn;
    [SerializeField] private Sprite _notificationOff;


    [Header("Localization")]
    private LocalizedString _localizedString;
    private LocalizedString _productionLocalizedString;
    private LocalizedString _buyLocalizedString;
    [SerializeField] private string _table = "Tabla1";
    private string _gemDescription = "gemDescription";
    private string _gemProduction = "gemProduction";
    private StringVariable _amountVariable;
    private StringVariable _amountProductionVariable;
    private IntVariable _amountToBuyVariable;

    public event UnityAction<int> OnBuyGeneratorClicked;
    private bool _isAvailableToBuy;

    private void Awake()
    {
        _localizedString = _descriptionLocalized.StringReference;
        _productionLocalizedString = _productionLocalized.StringReference;
        _buyLocalizedString = _buyAmountLocalized.StringReference;
        SetAmountVariable();
        SetAmountProductionVariable();
        SetAmountToBuyVariable();
        //OnCurrencyChangedEventBinding.Bind(CheckIfCanBuy);
        //OnCurrencyChangedEventBinding.Bind(UpdatePrice);
        //OnProductionChangedEventBinding.Bind(DisplayProductionText);
        //OnProductionChangedEventBinding.Bind(DisplayDescription);
        //OnProductionChangedEventBinding.Bind(DisplayPercentageText);
        _isAvailableToBuy = false;
    }

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(HandleBuyButton);


        ActivateButton(false);

        //if (_generator == null) return;

        ChangeAmountToBuy();
        CheckIfCanBuy();
        DisplayAmountOwned();
        DisplayProductionText();
        DisplayPriceText();
        DisplayDescription();
        DisplayPercentageText();
        SetNotificationButtonImage();
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveAllListeners();


    }

    private void OnDestroy()
    {
        _generatorRTS.Remove(gameObject);
        //OnCurrencyChangedEventListener.DeRegister(CheckIfCanBuy);
        //OnCurrencyChangedEventListener.DeRegister(UpdatePrice);
        //OnProductionChangedEventListener.DeRegister(DisplayProductionText);
        //OnProductionChangedEventListener.DeRegister(DisplayDescription);
        //OnProductionChangedEventListener.DeRegister(DisplayPercentageText);
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void SetGenerator(GeneratorSO generator)
    {
        //_generator = generator;
    }

    public void PrepareButton()
    {
        UpdatePrice();
        DisplayImage();
        DisplayAmountOwned();
        DisplayName();
        DisplayDescription();
        DisplayProductionText();
        DisplayPercentageText();
    }

    public void ChangeAmountToBuy()
    {
        //if (_generator == null) return;

        CalculateAmountToBuy();
        CheckIfCanBuy();
        DisplayPriceText();
    }

    public void HandleBuyButton()
    {
        OnBuyGeneratorClicked?.Invoke(_index);
    }

    // Called when pressing Notification Button
    public void ToggleNotification()
    {
        //if (_generator == null) return;

        //_generator.ShouldNotify = !_generator.ShouldNotify;

        //SetNotificationButtonImage();
        //_isAvailableToBuy = false;

        //if (!_generator.ShouldNotify)
        //{
        //    _generatorRTS.Remove(gameObject);
        //}
    }

    private void CheckIfCanBuy()
    {
        //if (_generator == null) return;

        //if (!_generator.IsUnlocked)
        //{
        //    _generator.CheckIfMeetRequirementsToUnlock(_totalCurrency.Value);
        //}
        //ActivateButton(_generator.IsUnlocked);
        //if (_generator.IsUnlocked)
        //{
        //    ToggleBuyButton(_totalCurrency.Value >= _generator.Cost.Value);
        //}


        //gameObject.SetActive(_generator.IsUnlocked);
    }

    private void ToggleBuyButton(bool val)
    {
        if (!val)
        {
            if (_buyButton == null) return;

            if (EventSystem.current.currentSelectedGameObject == _buyButton.gameObject)
            {
                UIManager.Instance.SetGeneratorShopDefaultButton();
            }
            if (_isAvailableToBuy)
            {
                _isAvailableToBuy = false;
                _generatorRTS.Remove(gameObject);
            }

        }
        else
        {
            //if (!_isAvailableToBuy && _generator.ShouldNotify)
            //{
            //    _generatorRTS.Add(gameObject);
            //}

            _isAvailableToBuy = true;
        }

        _buyButton.interactable = val;
    }

    private void ActivateButton(bool val)
    {
        if (_background == null)
        {
            return;
        }

        _background.enabled = val;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(val);
        }
    }

    private void DisplayImage()
    {
        //_generatorIcon.sprite = _generator.GetSprite();
    }

    private void DisplayAmountToBuy(int amount)
    {
        //if (_generator == null) return;
        _amountToBuyVariable.Value = amount;
        _buyAmountLocalized.RefreshString();
    }

    private void DisplayAmountOwned()
    {
        //_amountText.SetTextFormat("{0}", _generator.AmountOwned);
    }

    private void DisplayName()
    {
        //_nameLocalized.StringReference.SetReference(_table, _generator.Name);
        _nameLocalized.RefreshString();
    }

    private void DisplayDescription()
    {
        //if (_generator == null) return;
        _descriptionLocalized.StringReference.SetReference(_table, _gemDescription);
        //_amountVariable.Value = FormatNumber.FormatDouble(_generator.Production.Value).GetFormatNoDecimals();
        _descriptionLocalized.RefreshString();
    }

    private void DisplayPriceText()
    {
        //_priceText.SetTextFormat("{0}", _generator.CostFormatted.GetFormat());
    }

    private void DisplayProductionText()
    {
        //if (_generator == null)  return;
        

        _productionLocalized.StringReference.SetReference(_table, _gemProduction);
        //_amountProductionVariable.Value = _generator.ProductionFormatted.GetFormat();
        _productionLocalized.RefreshString();
    }

    private void DisplayPercentageText()
    {
        //if (_generator == null) return;
        //_percentageText.SetTextFormat("{0}%", _generator.ProductionPercentage.ToString("F2"));
    }

    private void UpdatePrice()
    {
        DisplayAmountToBuy(CalculateAmountToBuy());
        DisplayPriceText();
    }

    private int CalculateAmountToBuy()
    {
        int amount = 0;
        if (_amountToBuy.Value > 0)
        {
            amount = _amountToBuy.Value;
        }
        else
        {
            amount = 1;//_generator.CalculateMaxAmountToBuy(_totalCurrency.Value);
        }
        //_generator.GetBulkCost(amount > 0 ? amount : 1);
        return amount > 0 ? amount : 1;
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

    private void SetNotificationButtonImage()
    {
        //_notificationImage.sprite = _generator.ShouldNotify ? _notificationOn : _notificationOff;
    }
}
