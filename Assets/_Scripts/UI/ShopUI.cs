using System.Collections.Generic;
using UnityEngine;

public class ShopUI : Singleton<ShopUI>
{
    [SerializeField] private GameObject _shopItemButtonPrefab;
    [SerializeField] private Transform _shopItemParent;
    [SerializeField] private CurrencyManager _currencyManager;
    private List<GeneratorButtonController> _buttons;
    private int _amountToBuy;

    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnBuyGameEvent;
    [SerializeField] private IntGameEvent OnChangeBuyAmountEvent;
    [Header("Int Event Listener")]
    [SerializeField] private IntGameEventListener OnGeneratorAmountChangedListener;
    [Header("Double Event Listener")]
    [SerializeField] private DoubleGameEventListener OnCurrencyChangedListener;

    protected override void Awake()
    {
        _currencyManager = CurrencyManager.Instance;
    }

    private void OnEnable()
    {
        _currencyManager.OnLoadAllGenerators += PrepareUI;
        OnGeneratorAmountChangedListener.Register(UpdateButtonInfo);
        OnCurrencyChangedListener.Register(HandleButtonAvailable);

    }

    private void OnDisable()
    {
        _currencyManager.OnLoadAllGenerators -= PrepareUI;
    }

    private void PrepareUI(List<GeneratorSO> generators, int amountToBuy)
    {
        _amountToBuy = amountToBuy;
        _buttons = new();

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
