using System.Collections.Generic;
using UnityEngine;

public class ShopUI : Singleton<ShopUI>
{
    [Header("Generator")]
    [SerializeField] private GameObject _shopGeneratorButtonPrefab;
    [SerializeField] private Transform _shopGeneratorContent;
    [SerializeField] private GameObject _generatorCanvasGO;
    private List<GeneratorButtonController> _generatorButtons = new();
    private int _amountToBuy;

    [Header("Upgrade")]
    [SerializeField] private GameObject _shopUpgradeButtonPrefab;
    [SerializeField] private Transform _shopUpgradeContent;
    [SerializeField] private GameObject _UpgradeCanvasGO;
    private List<UpgradeButtonController> _upgradeButtons = new();



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
        OnListGeneratorLoadedListener.Register(PrepareUIGenerator);
        OnGeneratorAmountChangedListener.Register(UpdateButtonInfo);
        OnChangeBuyAmountEventListener.Register(ChangeAmountToBuy);
        OnOpenShopEvent?.RaiseEvent();
    }

    private void OnDisable()
    {
        OnListGeneratorLoadedListener.DeRegister(PrepareUIGenerator);
        OnGeneratorAmountChangedListener.DeRegister(UpdateButtonInfo);
        OnChangeBuyAmountEventListener.DeRegister(ChangeAmountToBuy);
    }

    private void PrepareUIGenerator(List<GeneratorSO> generators)
    {
        if (_amountToBuy < 1)
        {
            _amountToBuy = 1;
        }

        _upgradeButtons.Clear();

        for (int i = 0; i < generators.Count; i++)
        {
            GeneratorButtonController button = Instantiate(_shopGeneratorButtonPrefab).GetComponent<GeneratorButtonController>();
            button.transform.SetParent(_shopGeneratorContent, false);
            button.SetIndex(i);
            button.SetGenerator(generators[i]);
            generators[i].GetProductionRate();
            button.ChangeAmountToBuy(_amountToBuy);
            button.OnBuyGeneratorClicked += BuyGenerator;
            button.PrepareButton();
            _generatorButtons.Add(button);
        }

        _generatorCanvasGO.SetActive(false);
    }

    private void UpdateButtonInfo(int index)
    {
        _generatorButtons[index].PrepareButton();
    }

    private void BuyGenerator(int index)
    {
        OnBuyGameEvent.RaiseEvent(index);
    }

    private void ChangeAmountToBuy(int amount)
    {
        _amountToBuy = amount;

        foreach (var button in _generatorButtons)
        {
            button.ChangeAmountToBuy(amount);
        }
    }
}
