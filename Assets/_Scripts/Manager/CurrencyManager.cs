using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
{
    [SerializeField] private float _delayToGenerate = 1f;
    [Header("Double Variable")]
    [SerializeField] private DoubleVariableSO _totalCurrency;
    [SerializeField] private DoubleVariableSO _gameTotalCurrency;
    [Header("Int Variable")]
    [SerializeField] private IntVariableSO _amountToBuy;

    [Header("Float Variable")]
    [SerializeField] private FloatVariableSO _crystalOnGetMultiplier;
    [SerializeField] private FloatVariableSO _crystalTotalMultiplier;

    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnGeneratorAmountChangedEvent;
    [SerializeField] private IntGameEvent OnChangeBuyAmountEvent;
    [Header("Void Event")]
    [SerializeField] private VoidGameEvent OnCurrencyChangedEvent;
    [SerializeField] private VoidGameEvent OnUpgradeBoughtEvent;

    [Header("Void Event Listener")]
    [SerializeField] private VoidGameEventListener OnBuyEveryUpgradeEventListener;
    [SerializeField] private VoidGameEventListener OnProductionChangedEventListener;
    [Header("Int Event Listener")]
    [SerializeField] private IntGameEventListener OnChangeBuyAmountEventListener;
    [SerializeField] private IntGameEventListener OnBuyGeneratorGameEventListener;
    [SerializeField] private IntGameEventListener OnBuyUpgradeGameEventListener;
    [Header("Double Event Listener")]
    [SerializeField] private DoubleGameEventListener OnGainCurrencyListener;
    [Header("Formatted Number Event")]
    [SerializeField] private FormattedNumberGameEvent OnLoadCurrencyEvent;
    [SerializeField] private FormattedNumberGameEvent OnUpdateCurrencyFormatted;
    [SerializeField] private FormattedNumberGameEvent OnUpdateProductionFormatted;
    [Header("Generator List")]
    [SerializeField] private ListGeneratorSO _generators;
    [SerializeField] private ListUpgradeSO _upgrades;

    [Header("Save Data")]
    private FormattedNumber UpdateCurrencyFormatted;
    private FormattedNumber UpdateProductionFormatted;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        OnChangeBuyAmountEventListener.Register(ChangeAmountToBuy);
        OnBuyGeneratorGameEventListener.Register(BuyGenerator);
        OnBuyUpgradeGameEventListener.Register(BuyUpgrade);
        OnGainCurrencyListener.Register(GetCrystal);
        OnBuyEveryUpgradeEventListener.Register(BuyEveryUpgradeAvailable);
        OnProductionChangedEventListener.Register(UpdateProductionRate);
        StartCoroutine(GenerateIncome());
    }

    private void OnDisable()
    {
        OnChangeBuyAmountEventListener.DeRegister(ChangeAmountToBuy);
        OnBuyGeneratorGameEventListener.DeRegister(BuyGenerator);
        OnBuyUpgradeGameEventListener.DeRegister(BuyUpgrade);
        OnGainCurrencyListener.DeRegister(GetCrystal);
        OnBuyEveryUpgradeEventListener.DeRegister(BuyEveryUpgradeAvailable);
        OnProductionChangedEventListener.DeRegister(UpdateProductionRate);
    }

    private void UpdateTotal(double amount)
    {
        _totalCurrency.Value += amount;
        _totalCurrency.Value = Math.Round(_totalCurrency.Value, 1);
        _gameTotalCurrency.Value += amount;
    }

    private void IncrementPerSecond(double amount)
    {
        UpdateTotal(amount);
        OnCurrencyChangedEvent.RaiseEvent();
        OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted));
    }

    private void GetCrystal(double amount)
    {
        amount *= _crystalOnGetMultiplier.Value * _crystalTotalMultiplier.Value;
        UpdateTotal(amount);
        OnCurrencyChangedEvent.RaiseEvent();
        OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted));
    }

    public void SaveData(GameDataSO gameData)
    {
        gameData.SaveCurrency(_totalCurrency.Value, _gameTotalCurrency.Value, _amountToBuy.Value);

        List<GeneratorData> generatorDatas = gameData.GetClearGeneratorDatas();
        foreach (var generator in _generators.Generators)
        {
            generatorDatas.Add(new(generator.Guid, generator.AmountOwned, generator.TotalProduction, generator.IsUnlocked));
        }

        gameData.SaveGenerators(generatorDatas);

        List<UpgradeData> upgradeDatas = gameData.GetClearUpgradeDatas();
        foreach (var upgrade in _upgrades.Upgrades)
        {
            upgradeDatas.Add(new(upgrade.Guid, upgrade.IsRequirementMet, upgrade.IsApplied));
        }

        gameData.SaveUpgrades(upgradeDatas);
    }

    public void LoadData(GameDataSO gameData)
    {
        LoadCurrency(gameData.CurrencyData);
        LoadGenerators(gameData.Generators);
        LoadUpgrades(gameData.Upgrades);
        LoadOfflineReward(gameData.CurrencyData.LastActiveDateTime);

        UpdateCurrency();
        GetProductionRate();

        OnChangeBuyAmountEvent.RaiseEvent(_amountToBuy.Value);
        OnUpgradeBoughtEvent.RaiseEvent();
        OnLoadCurrencyEvent.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted));
    }

    private void BuyGenerator(int index)
    {
        if (_totalCurrency.Value >= _generators.Generators[index].BulkCost)
        {
            _totalCurrency.Value -= _generators.Generators[index].BulkCost;
            _generators.Generators[index].AddAmount(_amountToBuy.Value);
            _generators.Generators[index].GetBulkCost(_amountToBuy.Value);
            _generators.Generators[index].CalculateProductionRate();
            OnGeneratorAmountChangedEvent.RaiseEvent(index);
            GetProductionRate();
            UpdateCurrency();
            OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted));
        }
    }

    private void BuyUpgrade(int index)
    {
        if (_totalCurrency.Value >= _upgrades.Upgrades[index].Cost.Value)
        {
            _upgrades.Upgrades[index].BuyUpgrade(_totalCurrency.Value);
            _totalCurrency.Value -= _upgrades.Upgrades[index].Cost.Value;
            UpdateCurrency();
            GetProductionRate();
            OnUpgradeBoughtEvent.RaiseEvent();
            OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted));
        }
    }

    private void BuyEveryUpgradeAvailable()
    {
        int upgradesBought = 0;

        for (int i = 0; i < _upgrades.Upgrades.Count; i++)
        {
            if (!_upgrades.Upgrades[i].IsRequirementMet) continue;
            if (_upgrades.Upgrades[i].IsApplied) continue;
            if (_totalCurrency.Value < _upgrades.Upgrades[i].Cost.Value) continue;

            BuyUpgrade(i);
            upgradesBought++;
        }

        Debug.Log($"Has comprado {upgradesBought} mejoras");
    }

    private void ChangeAmountToBuy(int amount)
    {
        _amountToBuy.Value = amount;
    }

    private void UpdateProductionRate()
    {
        foreach (var generator in _generators.Generators)
        {
            generator.CalculateProductionRate();
        }
    }

    private double GetProductionRate()
    {
        double production = 0;

        foreach (var generator in _generators.Generators)
        {
            production += generator.GetProductionRate();
        }

        OnUpdateProductionFormatted.RaiseEvent(FormatNumber.FormatDouble(production, UpdateProductionFormatted));
        return production;
    }

    private void UpdateCurrency()
    {
        OnCurrencyChangedEvent.RaiseEvent();
    }

    private IEnumerator GenerateIncome()
    {
        while (true)
        {
            yield return Helpers.GetWaitForSeconds(_delayToGenerate);
            IncrementPerSecond(GetProductionRate());
        }
    }

    private void LoadCurrency(CurrencyData currencyData)
    {
        _totalCurrency.Initialize(currencyData.TotalCurrency);
        _gameTotalCurrency.Initialize(currencyData.GameTotalCurrency);
        _amountToBuy.Value = currencyData.AmountToBuy;
        ChangeAmountToBuy(_amountToBuy.Value);
    }

    private void LoadGenerators(List<GeneratorData> generatorDatas)
    {
        foreach (var generatorSO in _generators.Generators)
        {
            generatorSO.Initialize();
        }

        foreach (var generator in generatorDatas)
        {
            var generatorSO = _generators.Find(generator.Guid);
            generatorSO.SetAmount(generator.Amount);
            generatorSO.SetTotalProduction(generator.TotalProduction);
            generatorSO.IsUnlocked = generator.IsUnlocked;
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
            upgradeSO.IsRequirementMet = upgrade.IsRequirementMet;
            upgradeSO.ApplyUpgrade(upgrade.IsApplied);
        }
    }

    private void LoadOfflineReward(long lastActive)
    {
        long timeSpan = DateTime.Now.Ticks - lastActive;
        double seconds = Math.Round(TimeSpan.FromTicks(timeSpan).TotalSeconds);

        seconds = Math.Clamp(seconds, 0, 86400);

        double production = 0;

        foreach (var generator in _generators.Generators)
        {
            production += generator.GetProductionRate();
        }

        production *= seconds;

        Debug.Log($"produccion desde la ultima conexion: {production} - segundos transcurridos: {seconds}");
        IncrementPerSecond(production);

        OnUpdateProductionFormatted.RaiseEvent(FormatNumber.FormatDouble(production, UpdateProductionFormatted));

    }
}
