using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Cysharp.Text;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using System.Collections.Generic;

public class UpgradeButtonController : MonoBehaviour
{
    [Header("Run Time Set")]
    [SerializeField] private GameObjectRuntimeSetSO _upgradesRTS;

    [Header("Double Variable")]
    [SerializeField] private DoubleVariableSO _totalCurrency;

    [Header("Void Event Listener")]
    [SerializeField] private VoidGameEventListener OnCurrencyChangedEventListener;
    [SerializeField] private VoidGameEventListener OnUpgradeBoughtEventListener;

    [Header("Bool Event")]
    [SerializeField] private BoolGameEvent OnUpgradeAvailableToBuyEvent;

    [Header("Assigned Automatically")]
    [SerializeField][ReadOnly] private BaseUpgradeSO _upgrade;
    [SerializeField][ReadOnly] private int _index;

    [Header("Button Fields")]
    [SerializeField] private Image _background;
    [SerializeField] private LocalizeStringEvent _nameLocalized;
    [SerializeField] private LocalizeStringEvent _descriptionLocalized;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;
    private bool _isAvailableToBuy;
    private bool _isAlreadyBought;

    [Header("Localization")]
    private LocalizedString _localizedString;
    private LocalizedString _descriptionLocalizedString;
    [SerializeField] private string _table = "Tabla1";
    private LocalizedString _gemLocalized;
    private LocalizedString _gemDescriptionLocalized;

    private FloatVariable _amountVariable;

    public event UnityAction<int> OnBuyUpgradeClicked;


    private void Awake()
    {
        _isAlreadyBought = false;
        _localizedString = _nameLocalized.StringReference;
        _descriptionLocalizedString = _descriptionLocalized.StringReference;
        SetGemLocalized();
        SetGemDescriptionLocalized();
        SetAmountVariable();
        //OnCurrencyChangedEventListener.Register(CheckIfCanBuy);
        //OnUpgradeBoughtEventListener.Register(CheckIfBought);
        _isAvailableToBuy = false;

    }

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(HandleBuyButton);
        ActivateButton(false);

        if (_upgrade == null) return;

        CheckIfCanBuy();
        CheckIfBought();
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        //OnCurrencyChangedEventListener.DeRegister(CheckIfCanBuy);
        //OnUpgradeBoughtEventListener.DeRegister(CheckIfBought);
        _upgradesRTS.Remove(gameObject);
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void SetUpgrade(BaseUpgradeSO upgrade)
    {
        _upgrade = upgrade;
    }

    public void PrepareButton()
    {
        DisplayName();
        DisplayPriceText();
        DisplayDescriptionText();
    }

    public void HandleBuyButton()
    {
        _upgradesRTS.Remove(gameObject);
        OnBuyUpgradeClicked?.Invoke(_index);
    }

    private void CheckIfCanBuy()
    {
        if (_upgrade == null) return;

        if (_isAlreadyBought || !_upgrade.IsRequirementMet)
        {
            _upgradesRTS.Remove(gameObject);
        }

        if (_upgrade.IsApplied) return;

        if (!_upgrade.IsRequirementMet)
        {
            _upgrade.UnlockRequirements(_totalCurrency.Value);
        }

        ActivateButton(_upgrade.IsRequirementMet);
        if (_upgrade.IsRequirementMet)
        {
            ToggleBuyButton(_totalCurrency.Value >= _upgrade.Cost.Value);
        }

        gameObject.SetActive(_upgrade.IsRequirementMet);
    }

    private void CheckIfBought()
    {
        if (_upgrade == null) return;

        if (!_isAlreadyBought && _upgrade.IsApplied)
        {
            ActivateButton(false);
            _isAlreadyBought = true;
            _isAvailableToBuy = false;
            Destroy(gameObject);

        }
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

            _isAvailableToBuy = false;
            _upgradesRTS.Remove(gameObject);


        }
        else
        {
            if (!_isAvailableToBuy && !_upgrade.IsApplied)
            {
                _upgradesRTS.Add(gameObject);
            }

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

    private void DisplayName()
    {
        if (_upgrade is GemUpgradeSO)
        {
            _nameLocalized.StringReference.SetReference(_table, _upgrade.Name);
            _gemLocalized.SetReference(_table, (_upgrade as GemUpgradeSO).GeneratorName);
            _gemLocalized.RefreshString();
            _nameLocalized.RefreshString();
        }
        else
        {
            _nameLocalized.StringReference.SetReference(_table, _upgrade.Name);
            _nameLocalized.RefreshString();
        }
    }

    private void DisplayPriceText()
    {
        _priceText.SetTextFormat("{0}", _upgrade.GetCost().GetFormat());
    }

    private void DisplayDescriptionText()
    {
        _descriptionLocalized.StringReference.SetReference(_table, _upgrade.Description);
        _amountVariable.Value = (_upgrade.AmountToModify < 1) ? _upgrade.AmountToModify * 100 : _upgrade.AmountToModify;

        if (_upgrade is GemUpgradeSO)
        {
            _gemDescriptionLocalized.SetReference(_table, (_upgrade as GemUpgradeSO).GeneratorName);
            _gemDescriptionLocalized.RefreshString();
        }

        _descriptionLocalized.RefreshString();

    }

    private void SetGemLocalized()
    {
        if (!_localizedString.TryGetValue("gem", out var localizedVariable))
        {
            _gemLocalized = new LocalizedString();
            _localizedString.Add("gem", localizedVariable);
        }
        else
        {
            _gemLocalized = localizedVariable as LocalizedString;
        }
    }

    private void SetGemDescriptionLocalized()
    {
        if (!_descriptionLocalizedString.TryGetValue("gem", out var localizedVariable))
        {
            _gemDescriptionLocalized = new LocalizedString();
            _descriptionLocalizedString.Add("gem", localizedVariable);
        }
        else
        {
            _gemDescriptionLocalized = localizedVariable as LocalizedString;
        }
    }

    private void SetAmountVariable()
    {
        if (!_descriptionLocalizedString.TryGetValue("amount", out var variable))
        {
            _amountVariable = new FloatVariable();
            _descriptionLocalizedString.Add("amount", variable);
        }
        else
        {
            _amountVariable = variable as FloatVariable;
        }
    }

}
