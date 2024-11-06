using System.Collections.Generic;
using UnityEngine;

public class ShopUI : Singleton<ShopUI>
{
    [SerializeField] private GameObject _shopItemButtonPrefab;
    [SerializeField] private Transform _shopItemParent;
    private List<GeneratorButtonController> _buttons = new();
    private int _amountToBuy;

    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnBuyGameEvent;
    [SerializeField] private IntGameEvent OnChangeBuyAmountEvent;
    [Header("Int Event Listener")]
    [SerializeField] private IntGameEventListener OnGeneratorAmountChangedListener;
    [Header("Double Event Listener")]
    [SerializeField] private DoubleGameEventListener OnCurrencyChangedListener;
    [Header("List Generator Event Listener")]
    [SerializeField] private ListGeneratorGameEventListener OnListGeneratorLoadedListener;

    private void OnEnable()
    {
        OnListGeneratorLoadedListener.Register(PrepareUI);
        OnGeneratorAmountChangedListener.Register(UpdateButtonInfo);
        OnCurrencyChangedListener.Register(HandleButtonAvailable);
    }


    private void PrepareUI(List<GeneratorSO> generators)
    {
        _amountToBuy = 1;
        _buttons.Clear();

        for (int i = 0; i < generators.Count; i++)
        {
            GeneratorButtonController button = Instantiate(_shopItemButtonPrefab).GetComponent<GeneratorButtonController>();
            button.transform.SetParent(_shopItemParent, false);
            button.SetIndex(i);
            button.SetGenerator(generators[i]);
            generators[i].GetBulkCost(_amountToBuy);
            generators[i].GetProductionRate();
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
}
