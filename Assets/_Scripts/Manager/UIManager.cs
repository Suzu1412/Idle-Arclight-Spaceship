using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;

public class UIManager : Singleton<UIManager>
{
    private Coroutine _disableShopCoroutine;

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI _shopText;
    [SerializeField] private GameObject _shopGameObject;
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

    public void SetGeneratorShopDefaultButton()
    {
        _generatorShopDefaultButton.Select();
    }

    public void ChangeAmountToBuy(int amount)
    {
        OnChangeBuyAmountEvent?.RaiseEvent(amount);
    }

    public void BuyEveryUpgradeAvailable()
    {

    }

    public void OpenGeneratorStore()
    {
        _shopText.text = "Generator Store";
        _generatorShopUI.SetActive(true);
        _upgradeShopUI.SetActive(false);
    }

    public void OpenUpgradeStore()
    {
        _shopText.text = "Upgrade Store";
        _generatorShopUI.SetActive(false);
        _upgradeShopUI.SetActive(true);
    }

    private void ChangeSelectedAmountButton(int amount)
    {
        _buy1AmountImage.sprite = (amount == 1) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buy10AmountImage.sprite = (amount == 10) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buy50AmountImage.sprite = (amount == 50) ? _buttonAmountChecked : _buttonAmountUnchecked;
        _buy100AmountImage.sprite = (amount == 100) ? _buttonAmountChecked : _buttonAmountUnchecked;
    }

    private void ToggleShop(bool isActive)
    {
        _joystick.SetActive(!isActive);
        if (isActive)
        {
            if (_disableShopCoroutine != null) StopCoroutine(_disableShopCoroutine);
            _shopGameObject.SetActive(true);
            OpenGeneratorStore();
            _shopPanel.DOKill();
            _shopPanel.DOLocalMove(_openPosition, 0.4f).SetEase(Ease.InOutSine);
            SetGeneratorShopDefaultButton();
        }
        else
        {
            _shopPanel.DOKill();
            _shopPanel.DOLocalMove(_closePosition, 0.4f).SetEase(Ease.InOutSine);
            if (_disableShopCoroutine != null) StopCoroutine(_disableShopCoroutine);
            _disableShopCoroutine = StartCoroutine(DisableShopElements());
        }
    }

    private IEnumerator DisableShopElements()
    {
        yield return Helpers.GetWaitForSeconds(0.4f);
        _generatorShopUI.SetActive(false);

        _shopGameObject.SetActive(false);
    }

}
