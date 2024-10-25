using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
{
    [SerializeField] [ReadOnly] private List<GeneratorSO> _generators;
    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnGeneratorAmountChangedEvent;
    [Header("Double Event")]
    [SerializeField] private DoubleGameEvent OnCurrencyChangedEvent;
    [Header("Int Event Listener")]
    [SerializeField] private IntGameEventListener OnChangeBuyAmountEventListener;
    [SerializeField] private IntGameEventListener OnBuyGameEventListener;
    [Header("Double Event Listener")]
    [SerializeField] private DoubleGameEventListener OnGainCurrencyListener;
    [Header("String Event")]
    [SerializeField] private StringGameEvent OnUpdateCurrencyText;
    [SerializeField] private StringGameEvent OnUpdateProductionText;
    [Header("Save Data")]
    [SerializeField] private CurrencyData _currencyData;
    [SerializeField] private float _delayToGenerate = 1f;

    #region Events
    public event UnityAction<List<GeneratorSO>, int> OnLoadAllGenerators;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _currencyData = new CurrencyData();
    }

    private void OnEnable()
    {
        OnChangeBuyAmountEventListener.Register(ChangeAmountToBuy);
        OnBuyGameEventListener.Register(BuyGenerator);
        OnGainCurrencyListener.Register(Increment);
        StartCoroutine(GenerateIncome());
    }

    private void OnDisable()
    {
        OnChangeBuyAmountEventListener.DeRegister(ChangeAmountToBuy);
        OnBuyGameEventListener.DeRegister(BuyGenerator);
        OnGainCurrencyListener.DeRegister(Increment);
    }

    private void Increment(double amount)
    {
        _currencyData.TotalCurrency += amount;
        _currencyData.TotalCurrency = Math.Round(_currencyData.TotalCurrency, 1);
        OnCurrencyChangedEvent.RaiseEvent(_currencyData.TotalCurrency);
        OnUpdateCurrencyText.RaiseEvent(FormatNumber.FormatDouble(_currencyData.TotalCurrency));
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
        ChangeAmountToBuy(gameData.CurrencyData.AmountToBuy);

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

        OnLoadAllGenerators?.Invoke(_generators, _currencyData.AmountToBuy);
        OnCurrencyChangedEvent.RaiseEvent(_currencyData.TotalCurrency);
    }

    private void BuyGenerator(int index)
    {
        if (_currencyData.TotalCurrency >= _generators[index].Cost)
        {
            _currencyData.TotalCurrency -= _generators[index].Cost;
            _generators[index].AddAmount(_currencyData.AmountToBuy);
            _generators[index].GetBulkCost(_currencyData.AmountToBuy);
            _generators[index].CalculateProductionRate();
            GetProductionRate();
            OnGeneratorAmountChangedEvent.RaiseEvent(index);
            OnCurrencyChangedEvent.RaiseEvent(_currencyData.TotalCurrency);
            OnUpdateCurrencyText.RaiseEvent(FormatNumber.FormatDouble(_currencyData.TotalCurrency));
        }
    }

    private void ChangeAmountToBuy(int amount)
    {
        _currencyData.AmountToBuy = amount;

        if (_currencyData.AmountToBuy == 0)
        {
            _currencyData.AmountToBuy = 1;
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
            production += generator.GetProductionRate();
        }

        OnUpdateProductionText.RaiseEvent(FormatNumber.FormatDouble(production));

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
