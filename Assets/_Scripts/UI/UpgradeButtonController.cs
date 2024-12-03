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
using System.Reflection.Emit;

public class UpgradeButtonController : MonoBehaviour
{
    [Header("Double Variable")]
    [SerializeField] private DoubleVariableSO _totalCurrency;

    [Header("Void Event Listener")]
    [SerializeField] private VoidGameEventListener OnCurrencyChangedEventListener;
    [SerializeField] private VoidGameEventListener OnUpgradeBoughtEventListener;

    [Header("Assigned Automatically")]
    [SerializeField][ReadOnly] private BaseUpgradeSO _upgrade;
    [SerializeField][ReadOnly] private int _index;

    [Header("Button Fields")]
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;
    private bool _isAlreadyBought;

    public event UnityAction<int> OnBuyUpgradeClicked;

    private void Awake()
    {
        _isAlreadyBought = false;
        OnCurrencyChangedEventListener.Register(CheckIfCanBuy);
    }

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(HandleBuyButton);
        ActivateButton(false);

        if (_upgrade == null) return;

        CheckIfCanBuy();
        CheckIfBought();
        OnUpgradeBoughtEventListener.Register(CheckIfBought);
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveAllListeners();
        OnUpgradeBoughtEventListener.DeRegister(CheckIfBought);
    }

    private void OnDestroy()
    {
        OnCurrencyChangedEventListener.DeRegister(CheckIfCanBuy);
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
        OnBuyUpgradeClicked?.Invoke(_index);
    }

    private void CheckIfCanBuy()
    {
        if (_upgrade == null) return;

        if (_upgrade.IsApplied) return;

        if (_upgrade.IsRequirementMet || _totalCurrency.Value >= _upgrade.CostRequirement)
        {
            _upgrade.UnlockRequirements(_totalCurrency.Value);

        }

        ActivateButton(_upgrade.IsRequirementMet);
        ToggleBuyButton(_totalCurrency.Value >= _upgrade.Cost.Value);
        gameObject.SetActive(_upgrade.IsRequirementMet);
    }

    private void CheckIfBought()
    {
        if (_upgrade == null) return;

        if (!_isAlreadyBought && _upgrade.IsApplied)
        {
            ActivateButton(false);
            _isAlreadyBought = true;
            gameObject.SetActive(false);

        }
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

    private void DisplayName()
    {
        if (_upgrade is GemUpgradeSO)
        {

        }
        else
        {
            _nameText.SetTextFormat(format: "{0}", _upgrade.Name);

        }
    }

    private void DisplayPriceText()
    {
        _priceText.SetTextFormat("{0}", _upgrade.GetCost().GetFormat());
    }

    private void DisplayDescriptionText()
    {
        _descriptionText.SetTextFormat("{0}", _upgrade.Description);
    }
}
