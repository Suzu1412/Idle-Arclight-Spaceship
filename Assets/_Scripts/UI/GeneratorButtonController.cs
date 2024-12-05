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

public class GeneratorButtonController : MonoBehaviour
{
    [Header("Int Variable")]
    [SerializeField] private IntVariableSO _amountToBuy;
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
    [SerializeField] private LocalizeStringEvent _nameLocalized;
    [SerializeField] private LocalizeStringEvent _descriptionLocalized;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _productionText;
    [SerializeField] private Button _buyButton;
    [Header("Localization")]
    private LocalizedString _localizedString;
    [SerializeField] private string _table = "Tabla1";
    private string _gemDescription = "gemDescription";
    private StringVariable _amountVariable;

    public event UnityAction<int> OnBuyGeneratorClicked;

    private void Awake()
    {
        _localizedString = _descriptionLocalized.StringReference;
        SetAmountVariable();
        OnCurrencyChangedEventListener.Register(CheckIfCanBuy);
    }

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(HandleBuyButton);
        OnProductionChangedEventListener.Register(DisplayProductionText);
        OnProductionChangedEventListener.Register(DisplayDescription);

        ActivateButton(false);

        if (_generator == null) return;

        ChangeAmountToBuy();
        CheckIfCanBuy();
        DisplayAmountOwned();
        DisplayProductionText();
        DisplayPriceText();
        DisplayDescription();
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveAllListeners();
        OnProductionChangedEventListener.DeRegister(DisplayProductionText);
        OnProductionChangedEventListener.DeRegister(DisplayDescription);

    }

    private void OnDestroy()
    {
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
        DisplayDescription();
        DisplayPriceText();
        DisplayProductionText();
    }

    public void ChangeAmountToBuy()
    {
        if (_generator == null) return;
        _generator.GetBulkCost(_amountToBuy.Value);
        CheckIfCanBuy();
        DisplayPriceText();
    }

    public void HandleBuyButton()
    {
        OnBuyGeneratorClicked?.Invoke(_index);
    }

    private void CheckIfCanBuy()
    {
        if (_generator == null) return;

        if (!_generator.IsUnlocked)
        {
            _generator.CheckIfMeetRequirementsToUnlock(_totalCurrency.Value);
        }
        ActivateButton(_generator.IsUnlocked);
        ToggleBuyButton(_totalCurrency.Value >= _generator.Cost.Value);

        gameObject.SetActive(_generator.IsUnlocked);
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
        _nameLocalized.StringReference.SetReference(_table, _generator.Name);
        _nameLocalized.RefreshString();
    }

    private void DisplayDescription()
    {
        if (_generator == null) return;
        _descriptionLocalized.StringReference.SetReference(_table, _gemDescription);
        _amountVariable.Value = FormatNumber.FormatDouble(_generator.Production.Value).GetFormatNoDecimals();
        _descriptionLocalized.RefreshString();
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
}
