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
 
    [SerializeField] private BoolGameEventListener OnToggleShopEventListener;

    [Header("Joystick")]
    [SerializeField] private GameObject _joystick;

    [Header("Shop UI")]
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private Button _shopDefaultButton;

    private void Start()
    {
        _openPosition = Vector2.zero;
        _closePosition = new Vector2(_shopPanel.rect.width, 0);

        _shopPanel.anchoredPosition = _closePosition;
    }

    private void OnEnable()
    {
        OnToggleShopEventListener.Register(ToggleShop);
    }

    private void OnDisable()
    {
        OnToggleShopEventListener.DeRegister(ToggleShop);
    }

    public void SetShopDefaultButton()
    {
        _shopDefaultButton.Select();
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
