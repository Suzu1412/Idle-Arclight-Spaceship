using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [Header("Elements")]
    [SerializeField] private RectTransform _shopPanel;

    [Header("Settings")]
    [SerializeField] private Vector2 _openPosition;
    [SerializeField] private Vector2 _closePosition;

    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnChangeBuyAmountEvent;
    [Header("Bool Event Listener")]
    [SerializeField] private BoolGameEventListener OnToggleShopEventListener;
    [Header("Int Event Listener")]
    [SerializeField] private IntGameEventListener OnChangeBuyAmountEventListener;

    [Header("Joystick")]
    [SerializeField] private GameObject _joystick;

    [Header("Shop UI")]
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private Button _shopDefaultButton;
    [SerializeField] private Image _buy1AmountImage;
    [SerializeField] private Image _buy10AmountImage;
    [SerializeField] private Image _buy50AmountImage;
    [SerializeField] private Image _buy100AmountImage;

    [SerializeField] private Sprite _buttonAmountChecked;
    [SerializeField] private Sprite _buttonAmountUnchecked;

    private void Start()
    {
        _openPosition = Vector2.zero;
        _closePosition = new Vector2(_shopPanel.rect.width, 0);

        _shopPanel.anchoredPosition = _closePosition;
    }

    private void OnEnable()
    {
        OnToggleShopEventListener.Register(ToggleShop);
        OnChangeBuyAmountEventListener.Register(ChangeSelectedAmountButton);
    }

    private void OnDisable()
    {
        OnToggleShopEventListener.DeRegister(ToggleShop);
        OnChangeBuyAmountEventListener.DeRegister(ChangeSelectedAmountButton);

    }

    public void SetShopDefaultButton()
    {
        _shopDefaultButton.Select();
    }

    public void ChangeAmountToBuy(int amount)
    {
        OnChangeBuyAmountEvent?.RaiseEvent(amount);
    }

    private void ChangeSelectedAmountButton(int amount)
    {
        _buy1AmountImage.sprite = (amount == 1) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buy10AmountImage.sprite = (amount == 10) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buy50AmountImage.sprite = (amount == 50) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buy100AmountImage.sprite = (amount == 100) ? _buttonAmountChecked : _buttonAmountUnchecked;
    }

    private async void ToggleShop(bool isActive)
    {
        _joystick.SetActive(!isActive);
        if (isActive)
        {
            _shopUI.SetActive(isActive);
            _shopPanel.DOKill();
            _shopPanel.DOLocalMove(_openPosition, 0.4f).SetEase(Ease.InOutSine);
            SetShopDefaultButton();
        }
        else
        {
            _shopPanel.DOKill();
            _shopPanel.DOLocalMove(_closePosition, 0.4f).SetEase(Ease.InOutSine);
            await Awaitable.WaitForSecondsAsync(0.4f);
            _shopUI.SetActive(isActive);
        }
    }


}
