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

    [Header("List Generator Event Listener")]
    [SerializeField] private ListGeneratorGameEventListener OnListGeneratorLoadedListener;

    private void OnEnable()
    {
        OnListGeneratorLoadedListener.Register(PrepareUI);
        OnGeneratorAmountChangedListener.Register(UpdateButtonInfo);
        OnChangeBuyAmountEventListener.Register(ChangeAmountToBuy);
        OnOpenShopEvent?.RaiseEvent();
    }

    private void OnDisable()
    {
        OnListGeneratorLoadedListener.DeRegister(PrepareUI);
        OnGeneratorAmountChangedListener.DeRegister(UpdateButtonInfo);
        OnChangeBuyAmountEventListener.DeRegister(ChangeAmountToBuy);

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

    private void BuyGenerator(int index)
    {
        OnBuyGameEvent.RaiseEvent(index);
    }

    private void ChangeAmountToBuy(int amount)
    {
        _amountToBuy = amount;

        foreach (var button in _buttons)
        {
            button.ChangeAmountToBuy(amount);
        }
    }
}
