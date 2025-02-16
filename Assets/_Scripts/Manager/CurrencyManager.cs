using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
{
    [SerializeField] private SaveableRunTimeSetSO _saveable;

    [SerializeField] private CurrencyDataSO _currencyData;

    [Header("Int Variable")]
    [SerializeField] private IntVariableSO _amountToBuy;

    [Header("Float Variable")]
    [SerializeField] private FloatVariableSO _productionOfflineMultiplier;

    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnGeneratorAmountChangedEvent;
    [SerializeField] private IntGameEvent OnChangeBuyAmountEvent;
    [Header("Void Event")]
    [SerializeField] private VoidGameEvent OnCurrencyChangedEvent;
    [SerializeField] private VoidGameEvent OnUpgradeBoughtEvent;

    [Header("Void Event Binding")]
    [SerializeField] private VoidGameEventBinding OnBuyEveryUpgradeEventBinding;
    [SerializeField] private VoidGameEventBinding OnProductionChangedEventBinding;
    [Header("Int Event Listener")]
    [SerializeField] private IntGameEventListener OnChangeBuyAmountEventListener;
    [SerializeField] private IntGameEventListener OnBuyGeneratorGameEventListener;
    [SerializeField] private IntGameEventListener OnBuyUpgradeGameEventListener;

    [Header("Formatted Number Event")]
    [SerializeField] private FormattedNumberGameEvent OnLoadCurrencyEvent;
    [SerializeField] private FormattedNumberGameEvent OnUpdateCurrencyFormatted;
    [SerializeField] private FormattedNumberGameEvent OnUpdateProductionFormatted;
    [Header("Generator List")]
    [SerializeField] private GeneratorDatabaseSO _generatorDatabase;
    [SerializeField] private ListUpgradeSO _upgrades;

    [Header("Notification")]
    [SerializeField] private NotificationSO _offlineNotification;
    [SerializeField] private NotificationGameEvent _offlineNotificationEvent;


    [Header("Save Data")]
    private FormattedNumber UpdateCurrencyFormatted;
    private FormattedNumber UpdateProductionFormatted;

    // Actions
    private Action BuyEveryUpgradeAvailableAction;
    private Action UpdateProductionRateAction;


    protected override void Awake()
    {
        base.Awake();
        BuyEveryUpgradeAvailableAction = BuyEveryUpgradeAvailable;
        UpdateProductionRateAction = UpdateProductionRate;
        _generatorDatabase.InitializeDictionary();
    }

    private void OnEnable()
    {
        _saveable.Add(this);
        OnChangeBuyAmountEventListener.Register(ChangeAmountToBuy);
        OnBuyGeneratorGameEventListener.Register(BuyGenerator);
        OnBuyUpgradeGameEventListener.Register(BuyUpgrade);
        OnBuyEveryUpgradeEventBinding.Bind(BuyEveryUpgradeAvailableAction, this);
        OnProductionChangedEventBinding.Bind(UpdateProductionRateAction, this);
        StartCoroutine(GenerateIncome());
    }

    private void OnDisable()
    {
        _saveable.Remove(this);
        OnChangeBuyAmountEventListener.DeRegister(ChangeAmountToBuy);
        OnBuyGeneratorGameEventListener.DeRegister(BuyGenerator);
        OnBuyUpgradeGameEventListener.DeRegister(BuyUpgrade);
        OnBuyEveryUpgradeEventBinding.Unbind(BuyEveryUpgradeAvailableAction, this);
        OnProductionChangedEventBinding.Unbind(UpdateProductionRateAction, this);
    }

    private void IncrementPerSecond(BigNumber amount)
    {
        //UpdateTotal(amount);
        //OnCurrencyChangedEvent.RaiseEvent(this);
        //OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted), this);
    }


    public void SaveData(GameDataSO gameData)
    {
        gameData.CurrencyData = new(_currencyData.LifetimeCurrency, _currencyData.TotalCurrency, _currencyData.TotalProduction, _amountToBuy.Value);

        gameData.Generators.Clear();
        List<GeneratorData> generatorDatas = gameData.Generators;
        foreach (var generator in _generatorDatabase.GeneratorDictionary.Values)
        {
            generatorDatas.Add(new(generator.Guid, generator.AmountOwned, generator.TotalProduction, generator.IsUnlocked, generator.ShouldNotify));
        }

        gameData.Generators = generatorDatas;

        gameData.Upgrades.Clear();
        List<UpgradeData> upgradeDatas = gameData.Upgrades;
        foreach (var upgrade in _upgrades.Upgrades)
        {
            upgradeDatas.Add(new(upgrade.Guid, upgrade.IsRequirementMet, upgrade.IsApplied));
        }

        gameData.Upgrades = upgradeDatas;
    }

    public void LoadData(GameDataSO gameData)
    {
        LoadCurrency(gameData.CurrencyData);
        LoadGenerators(gameData.Generators);
        LoadUpgrades(gameData.Upgrades);
        _currencyData.CalculateOfflineEarnings(gameData.CurrencyData.LastActiveDateTime);
        //LoadOfflineReward(gameData.CurrencyData.LastActiveDateTime);

        OnChangeBuyAmountEvent.RaiseEvent(_amountToBuy.Value, this);
        OnUpgradeBoughtEvent.RaiseEvent(this);
        //OnLoadCurrencyEvent.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted), this);

        //for (int i = 0; i < _generatorDatabase.GeneratorDictionary.Count; i++)
        //{
        //    OnGeneratorAmountChangedEvent.RaiseEvent(i, this);
        //}
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void BuyGenerator(int index)
    {
        if (_currencyData.TotalCurrency >= _generatorDatabase.GeneratorDictionary[index].CostBigNumber)
        {
            int amountToBuy = 0;

            if (_amountToBuy.Value > 0)
            {
                amountToBuy = _amountToBuy.Value;
            }
            else
            {
                amountToBuy = 1;// _generators.Generators[index].CalculateMaxAmountToBuy(_totalCurrency.Value);
            }
            _generatorDatabase.GeneratorDictionary[index].GetBulkCost(amountToBuy);
            _currencyData.SubtractCurrency(_generatorDatabase.GeneratorDictionary[index].CostBigNumber);
            _generatorDatabase.GeneratorDictionary[index].AddAmount(amountToBuy);
            _generatorDatabase.GeneratorDictionary[index].CalculateProductionRate();
            GetProductionRate();
            UpdateCurrency();
            OnGeneratorAmountChangedEvent.RaiseEvent(index, this);
            //OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted), this);
        }
    }

    private void BuyUpgrade(int index)
    {
        //if (_currencyData.TotalCurrency.Value >= _upgrades.Upgrades[index].Cost.Value)
        //{
        //    _upgrades.Upgrades[index].BuyUpgrade(_totalCurrency.Value);
        //    _totalCurrency.Value -= _upgrades.Upgrades[index].Cost.Value;
        //    GetProductionRate();
        //    OnUpgradeBoughtEvent.RaiseEvent(this);
        //    OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted), this);
        //    UpdateCurrency();

        //}
    }

    private void BuyEveryUpgradeAvailable()
    {
        int upgradesBought = 0;

        //for (int i = 0; i < _upgrades.Upgrades.Count; i++)
        //{
        //    if (!_upgrades.Upgrades[i].IsRequirementMet) continue;
        //    if (_upgrades.Upgrades[i].IsApplied) continue;
        //    if (_totalCurrency.Value < _upgrades.Upgrades[i].Cost.Value) continue;

        //    BuyUpgrade(i);
        //    upgradesBought++;
        //}

        Debug.Log($"Has comprado {upgradesBought} mejoras");
    }

    private void ChangeAmountToBuy(int amount)
    {
        _amountToBuy.Value = amount;
        UpdateCurrency();
    }

    private void UpdateProductionRate()
    {
        foreach (var generator in _generatorDatabase.GeneratorDictionary.Values)
        {
            generator.IsDirty = true;
        }
        GetProductionRate();
    }

    private BigNumber GetProductionRate()
    {
        BigNumber totalProduction = BigNumber.Zero;

        foreach(var generator in _generatorDatabase.GeneratorDictionary.Values)
        {
            totalProduction += generator.ProductionBigNumber;
        }

        _currencyData.SetTotalProduction(totalProduction);

        foreach (var generator in _generatorDatabase.GeneratorDictionary.Values)
        {
            generator.CalculatePercentage(totalProduction);
        }

        //_generatorsTotalProduction.Value = 0;

        //foreach (var generator in _generators.Generators)
        //{
        //    _generatorsTotalProduction.Value += generator.GetProductionRate();
        //}

        //foreach (var generator in _generators.Generators)
        //{
        //    generator.CalculatePercentage();
        //}

        //OnUpdateProductionFormatted.RaiseEvent(FormatNumber.FormatDouble(_generatorsTotalProduction.Value, UpdateProductionFormatted), this);
        //return _generatorsTotalProduction.Value;
        return totalProduction;
    }

    private void UpdateCurrency()
    {
        OnCurrencyChangedEvent.RaiseEvent(this);
    }

    private IEnumerator GenerateIncome()
    {
        while (true)
        {
            yield return Helpers.WaitOneSecond;
            _currencyData.AddCurrency(_currencyData.TotalProduction);
        }
    }

    private void LoadCurrency(CurrencyData currencyData)
    {
        _currencyData.LoadCurrency(currencyData.LifetimeCurrency, currencyData.TotalCurrency, currencyData.HighestProduction);

        _amountToBuy.Value = currencyData.AmountToBuy;
        ChangeAmountToBuy(_amountToBuy.Value);
    }

    private void LoadGenerators(List<GeneratorData> generatorDatas)
    {
        foreach (var generatorSO in _generatorDatabase.GeneratorDictionary.Values)
        {
            generatorSO.Initialize();
        }

        foreach (var generator in generatorDatas)
        {
            var generatorSO = _generatorDatabase.Find(generator.Guid);
            if (generatorSO == null) return;
            generatorSO.SetAmount(generator.Amount);
            generatorSO.SetTotalProduction(generator.TotalProduction);
            generatorSO.IsUnlocked = generator.IsUnlocked;
            generatorSO.ShouldNotify = generator.ShouldNotify;
        }
    }

    private void LoadUpgrades(List<UpgradeData> upgradeDatas)
    {
        foreach (var upgradeSO in _upgrades.Upgrades)
        {
            upgradeSO.Initialize();
        }

        foreach (var upgrade in upgradeDatas)
        {
            var upgradeSO = _upgrades.Find(upgrade.Guid);
            if (upgradeSO == null) return;
            upgradeSO.IsRequirementMet = upgrade.IsRequirementMet;
            upgradeSO.ApplyUpgrade(upgrade.IsApplied);
        }
    }
}
