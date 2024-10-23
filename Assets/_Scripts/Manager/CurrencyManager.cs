using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
{
    [SerializeField] private List<GeneratorSO> _generators;
    [SerializeField] private List<GeneratorData> _generatorsData;
    [SerializeField] private DoubleGameEventListener _gainCurrencyListener;
    [SerializeField] private DoubleGameEvent _totalCurrency;
    [Header("Save Data")]
    [SerializeField] private CurrencyData _currencyData;
    [SerializeField] private float _delayToGenerate = 1f;

    protected override void Awake()
    {
        base.Awake();
        _currencyData = new CurrencyData();
    }

    private void OnEnable()
    {
        _gainCurrencyListener.Register(Increment);
        StartCoroutine(GenerateIncome());
    }

    private void OnDisable()
    {
        _gainCurrencyListener.DeRegister(Increment);
    }

    private void Increment(double amount)
    {
        _currencyData.TotalCurrency += amount;
        _currencyData.TotalCurrency = Math.Round(_currencyData.TotalCurrency, 1);
        _totalCurrency.RaiseEvent(_currencyData.TotalCurrency);
    }

    public void SaveData(GameData gameData)
    {
        gameData.CurrencyData = _currencyData;

        gameData.GeneratorsData = new();
        foreach (var generator in _generators)
        {
            var data = new GeneratorData
            {
                Guid = generator.Guid,
                Amount = generator.AmountOwned
            };
            gameData.GeneratorsData.Add(data);
        }
    }

    public void LoadData(GameData gameData)
    {
        _currencyData.TotalCurrency = gameData.CurrencyData.TotalCurrency;

        if (!gameData.GeneratorsData.IsNullOrEmpty())
        {
            _generators = new();
            foreach (var data in gameData.GeneratorsData)
            {
                var generator = GeneratorDataBase.GetAssetById(data.Guid);
                generator.SetAmount(data.Amount);
                _generators.Add(generator);
            }
        }
        else
        {
            _generators = GeneratorDataBase.GetAllAssets();
        }

        _totalCurrency.RaiseEvent(_currencyData.TotalCurrency);
    }

    private IEnumerator GenerateIncome()
    {
        while (true)
        {
            yield return Helpers.GetWaitForSeconds(_delayToGenerate);

            double amount = 0;

            foreach (var generator in _generators)
            {
                amount += generator.GetProductionRate();
            }

            Increment(amount);
        }
    }
}
