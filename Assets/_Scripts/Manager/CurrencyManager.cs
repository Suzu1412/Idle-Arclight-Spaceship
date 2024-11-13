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
    [SerializeField] private FloatVariableSO _generatorProductionMultiplier;
    [SerializeField] private FloatVariableSO _crystalOnGetMultiplier;
    [SerializeField] private FloatVariableSO _crystalTotalMultiplier;

    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnGeneratorAmountChangedEvent;
    [SerializeField] private IntGameEvent OnChangeBuyAmountEvent;
    [Header("Void Event")]
    [SerializeField] private VoidGameEvent OnCurrencyChangedEvent;
    [Header("Void Event Listener")]
    [SerializeField] private VoidGameEventListener OnOpenShopListener;
    [Header("Int Event Listener")]
    [SerializeField] private IntGameEventListener OnChangeBuyAmountEventListener;
    [SerializeField] private IntGameEventListener OnBuyGameEventListener;
    [Header("Double Event Listener")]
    [SerializeField] private DoubleGameEventListener OnGainCurrencyListener;
    [Header("Formatted Number Event")]
    [SerializeField] private FormattedNumberGameEvent OnLoadCurrencyEvent;
    [SerializeField] private FormattedNumberGameEvent OnUpdateCurrencyFormatted;
    [SerializeField] private FormattedNumberGameEvent OnUpdateProductionFormatted;
    [Header("List Generator Event")]
    [SerializeField] private ListGeneratorGameEvent OnListGeneratorEvent;
    [Header("Save Data")]
    [SerializeField] [ReadOnly] private List<GeneratorSO> _generators;
    [SerializeField] private int _amountToBuy = 1;
    private FormattedNumber UpdateCurrencyFormatted;
    private FormattedNumber UpdateProductionFormatted;

    [SerializeField] [Range(1f, 50f)] private float _generatorMultiplier = 1f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        OnChangeBuyAmountEventListener.Register(ChangeAmountToBuy);
        OnBuyGameEventListener.Register(BuyGenerator);
        OnGainCurrencyListener.Register(GetCrystal);
        OnOpenShopListener.Register(UpdateCurrency);
        StartCoroutine(GenerateIncome());
    }

    private void OnDisable()
    {
        OnChangeBuyAmountEventListener.DeRegister(ChangeAmountToBuy);
        OnBuyGameEventListener.DeRegister(BuyGenerator);
        OnGainCurrencyListener.DeRegister(GetCrystal);
        OnOpenShopListener.DeRegister(UpdateCurrency);
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
            var data = new GeneratorData(generator.Guid, generator.AmountOwned, generator.TotalProduction);
            gameData.GeneratorsData.Save(data);
        }
    }

    public void LoadData(GameData gameData)
    {
        gameData.CurrencyData ??= new();
        var currencyData = gameData.CurrencyData.Load();
        _totalCurrency.Initialize(currencyData.TotalCurrency);
        _amountToBuy = currencyData.AmountToBuy;
        ChangeAmountToBuy(gameData.CurrencyData.AmountToBuy);

        _generators = GeneratorDataBase.GetAllAssets();
        foreach (var generator in _generators)
        {
            generator.SetAmount(0);
            generator.SetTotalProduction(0);
            var data = gameData.GeneratorsData.Load(generator.Guid);
            if (data != null)
            {
                generator.SetAmount(data.Amount);
                generator.SetTotalProduction(data.TotalProduction);
            }
        }

        OnChangeBuyAmountEvent.RaiseEvent(_amountToBuy);
        OnListGeneratorEvent.RaiseEvent(_generators);
        UpdateCurrency();
        OnLoadCurrencyEvent.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted));
        GetProductionRate();
    }

    private void BuyGenerator(int index)
    {
        if (_totalCurrency.Value >= _generators[index].Cost)
        {
            _totalCurrency.Value -= _generators[index].Cost;
            _generators[index].AddAmount(_amountToBuy);
            _generators[index].GetBulkCost(_amountToBuy);
            _generators[index].CalculateProductionRate();
            GetProductionRate();
            OnGeneratorAmountChangedEvent.RaiseEvent(index);
            UpdateCurrency();
            OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency.Value, UpdateCurrencyFormatted));
        }
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
            production += generator.GetProductionRate() * _generatorProductionMultiplier.Value * _crystalTotalMultiplier.Value;
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
            Increment(GetProductionRate());
        }
    }
}
