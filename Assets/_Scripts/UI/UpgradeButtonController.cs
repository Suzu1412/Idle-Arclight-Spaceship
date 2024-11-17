using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UpgradeButtonController : MonoBehaviour
{
    [Header("Double Variable")]
    [SerializeField] private DoubleVariableSO _totalCurrency;

    [Header("Void Event Listener")]
    [SerializeField] private VoidGameEventListener OnCurrencyChangedEventListener;

    [Header("Assigned Automatically")]
    [SerializeField][ReadOnly] private BaseUpgradeSO _upgrade;
    [SerializeField][ReadOnly] private int _index;

    [Header("Button Fields")]
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;
    private bool _isAlreadyActive;

    public event UnityAction<int> OnBuyUpgradeClicked;

    private void Awake()
    {
        _isAlreadyActive = false;
        ActivateButton(_isAlreadyActive);
    }

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
        if (!_isAlreadyActive)
        {
            if (_upgrade.IsUnlocked || _totalCurrency.Value >= _upgrade.CostRequirement)
            {
                _isAlreadyActive = true;
                _upgrade.UnlockUpgrade(_totalCurrency.Value);
                ActivateButton(_isAlreadyActive);
            }
        }

        ToggleBuyButton(_totalCurrency.Value >= _upgrade.Cost.Value);
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
        _background.enabled = val;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(val);
        }

    }

    private void DisplayName()
    {
        _nameText.text = _upgrade.Name.ToString();
    }

    private void DisplayPriceText()
    {
        _priceText.text = _upgrade.GetCost().GetFormat();
    }

    private void DisplayDescriptionText()
    {
        _descriptionText.text = _upgrade.Description.ToString();
    }
}
