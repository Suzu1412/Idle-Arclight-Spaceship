using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
{
    [SerializeField] private float _delayToGenerate = 1f;
    [Header("Float Variable")]
    [SerializeField] private FloatVariableSO _generatorProductionMultiplier;
    [SerializeField] private FloatVariableSO _crystalOnGetMultiplier;
    [SerializeField] private FloatVariableSO _crystalTotalMultiplier;

    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnGeneratorAmountChangedEvent;
    [Header("Double Event")]
    [SerializeField] private DoubleGameEvent OnCurrencyChangedEvent;
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
    [SerializeField] private double _totalCurrency = 0;
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
        StartCoroutine(GenerateIncome());
    }

    private void OnDisable()
    {
        OnChangeBuyAmountEventListener.DeRegister(ChangeAmountToBuy);
        OnBuyGameEventListener.DeRegister(BuyGenerator);
        OnGainCurrencyListener.DeRegister(GetCrystal);
    }

    private void Increment(double amount)
    {
        _totalCurrency += amount;
        _totalCurrency = Math.Round(_totalCurrency, 1);
        OnCurrencyChangedEvent.RaiseEvent(_totalCurrency);
        OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency, UpdateCurrencyFormatted));
    }

    private void GetCrystal(double amount)
    {
        amount *= _crystalOnGetMultiplier.Value * _crystalTotalMultiplier.Value;
        Increment(amount);
    }

    public void SaveData(GameData gameData)
    {
        gameData.CurrencyData.Save(_totalCurrency, _amountToBuy);

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
        _totalCurrency = currencyData.TotalCurrency;
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

        OnListGeneratorEvent.RaiseEvent(_generators);
        OnCurrencyChangedEvent.RaiseEvent(_totalCurrency);
        OnLoadCurrencyEvent.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency, UpdateCurrencyFormatted));
        GetProductionRate();
    }

    private void BuyGenerator(int index)
    {
        if (_totalCurrency >= _generators[index].Cost)
        {
            _totalCurrency -= _generators[index].Cost;
            _generators[index].AddAmount(_amountToBuy);
            _generators[index].GetBulkCost(_amountToBuy);
            _generators[index].CalculateProductionRate();
            GetProductionRate();
            OnGeneratorAmountChangedEvent.RaiseEvent(index);
            OnCurrencyChangedEvent.RaiseEvent(_totalCurrency);
            OnUpdateCurrencyFormatted.RaiseEvent(FormatNumber.FormatDouble(_totalCurrency, UpdateCurrencyFormatted));
        }
    }

    private void ChangeAmountToBuy(int amount)
    {
        _amountToBuy = amount;

        if (_amountToBuy == 0)
        {
            _amountToBuy = 1;
        }

        foreach (var generator in _generators)
        {
            generator.GetBulkCost(amount);
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

    private IEnumerator GenerateIncome()
    {
        while (true)
        {
            yield return Helpers.GetWaitForSeconds(_delayToGenerate);
            Increment(GetProductionRate());
        }
    }
}
