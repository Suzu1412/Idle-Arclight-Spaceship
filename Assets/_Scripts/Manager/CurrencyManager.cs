using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
{
    [SerializeField] private List<GeneratorSO> _generators;
    [SerializeField] private DoubleGameEventListener _gainCurrencyListener;
    [SerializeField] private DoubleGameEvent _totalCurrency;
    [Header("Save Data")]
    [SerializeField] private CurrencyData _currencyData;
    [SerializeField] private float _delayToGenerate = 1f;
    private int _amountToBuy;

    #region Events
    public event UnityAction<List<GeneratorSO>> OnGeneratorLoad;
    public event UnityAction<double> OnCurrencyChanged;
    public event UnityAction<GeneratorSO> OnGeneratorAmountChanged;
    #endregion

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

        _generators = GeneratorDataBase.GetAllAssets();
        foreach (var generator in _generators)
        {
            generator.SetAmount(0);
        }

        if (!gameData.GeneratorsData.IsNullOrEmpty())
        {
            foreach (var data in gameData.GeneratorsData)
            {
                var newGenerator = GeneratorDataBase.GetAssetById(data.Guid);
                _generators.Find(x => x.Guid == newGenerator.Guid).SetAmount(data.Amount);
            }
        }

        OnGeneratorLoad?.Invoke(_generators);
        _totalCurrency.RaiseEvent(_currencyData.TotalCurrency);
    }

    public void BuyGenerator(GeneratorSO generator)
    {
        _generators.Find(x => x.Guid == generator.Guid).AddAmount(1);

    }

    private IEnumerator GenerateIncome()
    {
        while (true)
        {
            yield return Helpers.GetWaitForSeconds(_delayToGenerate);

            double income = 0;

            foreach (var generator in _generators)
            {
                income += generator.GetProductionRate();
            }

            Increment(income);
        }
    }
}
