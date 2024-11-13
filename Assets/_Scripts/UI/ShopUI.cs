using System.Collections.Generic;
using UnityEngine;

public class ShopUI : Singleton<ShopUI>
{
    [SerializeField] private GameObject _shopItemButtonPrefab;
    [SerializeField] private Transform _shopItemParent;
    private List<GeneratorButtonController> _buttons = new();
    private int _amountToBuy;

    [Header("Void Event")]
    [SerializeField] private VoidGameEvent OnOpenShopEvent;
    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnBuyGameEvent;
    [SerializeField] private IntGameEvent OnChangeBuyAmountEvent;
    [Header("Int Event Listener")]
    [SerializeField] private IntGameEventListener OnGeneratorAmountChangedListener;
    [SerializeField] private IntGameEventListener OnChangeBuyAmountEventListener;

    [Header("Double Event Listener")]
    [SerializeField] private DoubleGameEventListener OnCurrencyChangedListener;
    [Header("List Generator Event Listener")]
    [SerializeField] private ListGeneratorGameEventListener OnListGeneratorLoadedListener;

    private void OnEnable()
    {
        OnChangeBuyAmountEventListener.Register(ChangeAmountToBuy);
        OnListGeneratorLoadedListener.Register(PrepareUI);
        OnGeneratorAmountChangedListener.Register(UpdateButtonInfo);
        OnCurrencyChangedListener.Register(HandleButtonAvailable);
        OnOpenShopEvent?.RaiseEvent();
    }

    private void OnDisable()
    {
        OnChangeBuyAmountEventListener.DeRegister(ChangeAmountToBuy);
        OnListGeneratorLoadedListener.DeRegister(PrepareUI);
        OnGeneratorAmountChangedListener.DeRegister(UpdateButtonInfo);
        OnCurrencyChangedListener.DeRegister(HandleButtonAvailable);
    }


    private void PrepareUI(List<GeneratorSO> generators)
    {
        if (_amountToBuy < 1)
        {
            _amountToBuy = 1;
        }

        _buttons.Clear();

        for (int i = 0; i < generators.Count; i++)
        {
            GeneratorButtonController button = Instantiate(_shopItemButtonPrefab).GetComponent<GeneratorButtonController>();
            button.transform.SetParent(_shopItemParent, false);
            button.SetIndex(i);
            button.SetGenerator(generators[i]);
            generators[i].GetProductionRate();
            button.ChangeAmountToBuy(_amountToBuy);
            button.OnBuyGeneratorClicked += BuyGenerator;
            button.PrepareButton();
            _buttons.Add(button);
        }

        gameObject.SetActive(false);
    }

    private void UpdateButtonInfo(int index)
    {
        _buttons[index].PrepareButton();
    }

    private void HandleButtonAvailable(double currency)
    {
        foreach (var button in _buttons)
        {
            button.ToggleBuyButton(currency >= button.Cost);
        }
    }

    private void BuyGenerator(int index)
    {
        OnBuyGameEvent.RaiseEvent(index);
    }

    private void ChangeAmountToBuy(int amount)
    {
        _amountToBuy = amount;
        Debug.Log("Updating amount to Buy");

        foreach(var button in _buttons)
        {
            button.ChangeAmountToBuy(amount);
        }
    }
}
