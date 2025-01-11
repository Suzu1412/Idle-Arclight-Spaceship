using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cysharp.Text;

public class UIManager : Singleton<UIManager>
{
    private Coroutine _disableShopCoroutine;
    private Coroutine _disableMenuCoroutine;

    [Header("Shop Elements")]
    [SerializeField] private GameObject _generatorShopTitle;
    [SerializeField] private GameObject _upgradeShopTitle;

    [SerializeField] private GameObject _shopGameObject;
    [SerializeField] private RectTransform _shopPanel;

    [SerializeField] private Canvas _shopCanvas;

    [Header("Menu Elements")]
    [SerializeField] private GameObject _menuGameObject;
    [SerializeField] private RectTransform _menuPanel;

    [Header("Settings")]
    [SerializeField] private Vector2 _openPosition;
    [SerializeField] private Vector2 _closePosition;

    [Header("Void Event")]
    [SerializeField] private VoidGameEvent OnBuyEveryUpgradeEvent;
    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnChangeBuyAmountEvent;
    [Header("Bool Event Listener")]
    [SerializeField] private BoolGameEventListener OnToggleShopEventListener;
    [SerializeField] private BoolGameEventListener OnToggleMenuEventListener;
    [Header("Int Event Listener")]
    [SerializeField] private IntGameEventListener OnChangeBuyAmountEventListener;

    [Header("Joystick")]
    [SerializeField] private GameObject _joystick;

    [Header("Generator UI")]
    [SerializeField] private GameObject _generatorShopUI;
    [SerializeField] private Button _generatorShopDefaultButton;
    [SerializeField] private Image _buy1AmountImage;
    [SerializeField] private Image _buy10AmountImage;
    [SerializeField] private Image _buy50AmountImage;
    [SerializeField] private Image _buy100AmountImage;
    [SerializeField] private Sprite _buttonAmountChecked;
    [SerializeField] private Sprite _buttonAmountUnchecked;

    [Header("Upgrade UI")]
    [SerializeField] private GameObject _upgradeShopUI;
    [SerializeField] private Button _upgradeShopDefaultButton;

    [Header("Menu UI")]
    [SerializeField] private Button _menuDefaultButton;
    [SerializeField] private GameObject _languageSelectorPopup;
    [SerializeField] private GameObject _deleteDataConfirmPopup;


    private void Start()
    {
        _openPosition = Vector2.zero;
        _closePosition = new Vector2(_shopPanel.rect.width, 0);

        _shopPanel.anchoredPosition = _closePosition;

        _menuGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnToggleShopEventListener.Register(ToggleShop);
        OnToggleMenuEventListener.Register(ToggleMenu);
        OnChangeBuyAmountEventListener.Register(ChangeSelectedAmountButton);
    }

    private void OnDisable()
    {
        OnToggleShopEventListener.DeRegister(ToggleShop);
        OnToggleMenuEventListener.DeRegister(ToggleMenu);
        OnChangeBuyAmountEventListener.DeRegister(ChangeSelectedAmountButton);
    }

    public void SetGeneratorShopDefaultButton()
    {
        _generatorShopDefaultButton.Select();
    }

    public void SetSettingMenuDefaultButton()
    {
        _menuDefaultButton.Select();
    }

    public void ChangeAmountToBuy(int amount)
    {
        OnChangeBuyAmountEvent.RaiseEvent(amount);
    }

    public void BuyEveryUpgradeAvailable()
    {
        OnBuyEveryUpgradeEvent.RaiseEvent();
    }

    public void OpenGeneratorStore()
    {
        _generatorShopTitle.SetActive(true);
        _upgradeShopTitle.SetActive(false);
        _generatorShopUI.SetActive(true);
        _upgradeShopUI.SetActive(false);
    }

    public void OpenUpgradeStore()
    {
        _generatorShopTitle.SetActive(false);
        _upgradeShopTitle.SetActive(true);
        _generatorShopUI.SetActive(false);
        _upgradeShopUI.SetActive(true);
    }

    private void ChangeSelectedAmountButton(int amount)
    {
        _buy1AmountImage.sprite = (amount == 1) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buy10AmountImage.sprite = (amount == 5) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buy50AmountImage.sprite = (amount == 10) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buy100AmountImage.sprite = (amount == -1) ? _buttonAmountChecked : _buttonAmountUnchecked;
    }

    private void ToggleShop(bool isActive)
    {
        _joystick.SetActive(!isActive);
        if (isActive)
        {
            if (_disableShopCoroutine != null) StopCoroutine(_disableShopCoroutine);
            _shopCanvas.transform.localPosition = Vector3.zero;
            OpenGeneratorStore();
            //_shopPanel.DOKill();
            //_shopPanel.DOLocalMove(_openPosition, 0.4f).SetEase(Ease.InOutSine);
            _shopPanel.transform.localPosition = _openPosition;
            SetGeneratorShopDefaultButton();
        }
        else
        {
            //_shopPanel.DOKill();
            //_shopPanel.DOLocalMove(_closePosition, 0.4f).SetEase(Ease.InOutSine);
            _shopCanvas.transform.position = new Vector3(4000, 0, 0);
            _shopPanel.transform.localPosition = _closePosition;
            if (_disableShopCoroutine != null) StopCoroutine(_disableShopCoroutine);
            _disableShopCoroutine = StartCoroutine(DisableShopElements());

        }
    }

    private void ToggleMenu(bool isActive)
    {
        _joystick.SetActive(!isActive);
        if (isActive)
        {
            if (_disableMenuCoroutine != null) StopCoroutine(_disableMenuCoroutine);
            _menuGameObject.SetActive(true);
            _languageSelectorPopup.SetActive(false);
            _deleteDataConfirmPopup.SetActive(false);
            //_menuPanel.DOKill();
            //_menuPanel.DOLocalMove(_openPosition, 0.4f).SetEase(Ease.InOutSine);
            _menuPanel.transform.localPosition = _openPosition;
            SetSettingMenuDefaultButton();
        }
        else
        {
            //_menuPanel.DOKill();
            //_menuPanel.DOLocalMove(_closePosition, 0.4f).SetEase(Ease.InOutSine);
            _menuPanel.transform.localPosition = _closePosition;
            if (_disableMenuCoroutine != null) StopCoroutine(_disableMenuCoroutine);
            _disableMenuCoroutine = StartCoroutine(DisableMenuElements());
        }
    }

    private IEnumerator DisableShopElements()
    {
        yield return Helpers.GetWaitForSeconds(0.2f);
        //_generatorShopUI.SetActive(false);
        //_upgradeShopUI.SetActive(false);

        //_shopGameObject.SetActive(false);
    }

    private IEnumerator DisableMenuElements()
    {
        yield return Helpers.GetWaitForSeconds(0.2f);
        _menuGameObject.SetActive(false);
    }

}
