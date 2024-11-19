using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
{
    [SerializeField] private float _delayToGenerate = 1f;
    [Header("Double Variable")]
    [SerializeField] private DoubleVariableSO _totalCurrency;
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
    [Header("List Generator Event")]
    [SerializeField] private ListGeneratorGameEvent OnListGeneratorEvent;
    [Header("List Upgrade Event")]
    [SerializeField] private ListUpgradeGameEvent OnListUpgradeEvent;

    [Header("Save Data")]
    [SerializeField] [ReadOnly] private List<GeneratorSO> _generators;
    [SerializeField] [ReadOnly] private List<BaseUpgradeSO> _upgrades;
    [SerializeField] private int _amountToBuy = 1;
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
        StartCoroutine(GenerateIncome());
    }

    private void OnDisable()
    {
        OnChangeBuyAmountEventListener.DeRegister(ChangeAmountToBuy);
        OnBuyGeneratorGameEventListener.DeRegister(BuyGenerator);
        OnBuyUpgradeGameEventListener.DeRegister(BuyUpgrade);
        OnGainCurrencyListener.DeRegister(GetCrystal);
        OnBuyEveryUpgradeEventListener.DeRegister(BuyEveryUpgradeAvailable);

    }

    private void Increment(double amount)
    {
        _totalCurrency.Value += amount;
        _totalCurrency.Value = Math.Round(_totalCurrency.Value, 1);
        OnCurrencyChangedEvent.RaiseEvent();
        OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted));
    }

    private void GetCrystal(double amount)
    {
        amount *= _crystalOnGetMultiplier.Value * _crystalTotalMultiplier.Value;
        Increment(amount);
    }

    public void SaveData(GameData gameData)
    {
        gameData.CurrencyData.Save(_totalCurrency.Value, _amountToBuy);

        foreach (var generator in _generators)
        {
            var data = new GeneratorData(generator.Guid, generator.AmountOwned, generator.TotalProduction, generator.IsUnlocked);
            gameData.GeneratorsData.Save(data);
        }

        foreach (var upgrade in _upgrades)
        {
            var data = new UpgradeData(upgrade.Guid, upgrade.IsRequirementMet, upgrade.IsApplied);
            gameData.UpgradesData.Save(data);
        }
    }

    public void LoadData(GameData gameData)
    {
        gameData.CurrencyData ??= new();
        var currencyData = gameData.CurrencyData.Load();
        _totalCurrency.Initialize(currencyData.TotalCurrency);
        _amountToBuy = currencyData.AmountToBuy;
        ChangeAmountToBuy(gameData.CurrencyData.AmountToBuy);
        OnChangeBuyAmountEvent.RaiseEvent(_amountToBuy);

        LoadGenerators(gameData);
        LoadUpgrades(gameData);

        UpdateCurrency();
        OnLoadCurrencyEvent.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted));
        GetProductionRate();
    }

    private void BuyGenerator(int index)
    {
        if (_totalCurrency.Value >= _generators[index].BulkCost)
        {
            _totalCurrency.Value -= _generators[index].BulkCost;
            _generators[index].AddAmount(_amountToBuy);
            _generators[index].GetBulkCost(_amountToBuy);
            _generators[index].CalculateProductionRate();
            GetProductionRate();
            OnGeneratorAmountChangedEvent.RaiseEvent(index);
            UpdateCurrency();
            OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted));
        }
    }

    private void BuyUpgrade(int index)
    {
        if (_totalCurrency.Value >= _upgrades[index].Cost.Value)
        {
            _upgrades[index].BuyUpgrade(_totalCurrency.Value);
            _totalCurrency.Value -= _upgrades[index].Cost.Value;

            UpdateCurrency();
            GetProductionRate();
            OnUpgradeBoughtEvent.RaiseEvent();
            OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted));
        }
    }

    private void BuyEveryUpgradeAvailable()
    {
        int upgradesBought = 0;

        for (int i = 0; i < _upgrades.Count; i++)
        {
            if (!_upgrades[i].IsRequirementMet) continue;
            if (_upgrades[i].IsApplied) continue;
            if (_totalCurrency.Value < _upgrades[i].Cost.Value) continue;

            BuyUpgrade(i);
            upgradesBought++;
        }

        Debug.Log($"Has comprado {upgradesBought} mejoras");
    }

    private void ChangeAmountToBuy(int amount)
    {
        _amountToBuy = amount;

        if (_amountToBuy <= 0)
        {
            _amountToBuy = 1;
        }
    }

    private double GetProductionRate()
    {
        double production = 0;

        foreach (var generator in _generators)
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

    private void LoadGenerators(GameData gameData)
    {
        _generators = GeneratorDataBase.GetAllAssets();
        foreach (var generator in _generators)
        {
            generator.SetAmount(0);
            generator.SetTotalProduction(0);
            generator.IsUnlocked = false;
            var data = gameData.GeneratorsData.Load(generator.Guid);
            if (data != null)
            {
                generator.SetAmount(data.Amount);
                generator.SetTotalProduction(data.TotalProduction);
                generator.IsUnlocked = data.IsUnlocked;
            }
        }
        OnListGeneratorEvent.RaiseEvent(_generators);
    }

    private void LoadUpgrades(GameData gameData)
    {
        _upgrades = UpgradeDatabase.GetAllAssets();
        foreach (var upgrade in _upgrades)
        {
            upgrade.IsRequirementMet = false;
            var data = gameData.UpgradesData.Load(upgrade.Guid);
            if (data != null)
            {
                upgrade.IsRequirementMet = data.IsRequirementMet;
                upgrade.ApplyUpgrade(data.IsApplied);
            }
        }
        OnListUpgradeEvent.RaiseEvent(_upgrades);
        OnUpgradeBoughtEvent.RaiseEvent();
    }

    private IEnumerator GenerateIncome()
    {
        while (true)
        {
            yield return Helpers.GetWaitForSeconds(_delayToGenerate);
            Increment(GetProductionRate());
        }
    }
}
