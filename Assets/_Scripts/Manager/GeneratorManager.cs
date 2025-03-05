using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour, ISaveable
{
    [SerializeField] private SaveableRunTimeSetSO _saveable;
    [SerializeField] private CurrencyDataSO _currencyData;


    [Header("Int Variable")]
    [SerializeField] private IntVariableSO _amountToBuy;

    [Header("Int Event")]
    [SerializeField] private IntGameEvent OnChangeBuyAmountEvent;

    [Header("Void Event Binding")]
    [SerializeField] private VoidGameEventBinding OnProductionChangedEventBinding;

    [Header("Int Event Binding")]
    [SerializeField] private IntGameEventBinding OnChangeBuyAmountEventBinding;

    [Header("Generator List")]
    [SerializeField] private GeneratorDatabaseSO _generatorDatabase;

    private Action<int> ChangeAmountToBuyAction;
    private Action UpdateProductionRateAction;


    private void Awake()
    {
        _generatorDatabase.InitializeDictionary();
        ChangeAmountToBuyAction = ChangeAmountToBuy;
        UpdateProductionRateAction = UpdateProductionRate;
    }

    private void OnEnable()
    {
        _saveable.Add(this);
        OnChangeBuyAmountEventBinding.Bind(ChangeAmountToBuyAction, this);
        OnProductionChangedEventBinding.Bind(UpdateProductionRateAction, this);

    }

    private void OnDisable()
    {
        _saveable.Remove(this);
        OnChangeBuyAmountEventBinding.Unbind(ChangeAmountToBuyAction, this);
        OnProductionChangedEventBinding.Unbind(UpdateProductionRateAction, this);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void LoadData(GameDataSO gameData)
    {
        LoadGenerators(gameData.Generators);
        _currencyData.CalculateOfflineEarnings(gameData.CurrencyData.LastActiveDateTime);
        ChangeAmountToBuy(gameData.AmountToBuy);
    }

    public void SaveData(GameDataSO gameData)
    {
        gameData.SetAmountToBuy(_amountToBuy.Value);
        gameData.Generators.Clear();
        List<GeneratorData> generatorDatas = gameData.Generators;
        foreach (var generator in _generatorDatabase.GeneratorDictionary.Values)
        {
            generatorDatas.Add(new(generator.Guid, generator.AmountOwned, generator.TotalProduction, generator.IsVisibleInStore));
        }

        gameData.Generators = generatorDatas;
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
            generatorSO.IsVisibleInStore = generator.IsVisibleInStore;
        }
    }

    private void ChangeAmountToBuy(int amount)
    {
        _amountToBuy.Value = amount;
        OnChangeBuyAmountEvent.RaiseEvent(_amountToBuy.Value, this);

    }

    private void UpdateProductionRate()
    {
        foreach (var generator in _generatorDatabase.GeneratorDictionary.Values)
        {
            generator.HasProductionChanged = true;
        }
        GetProductionRate();
    }

    private BigNumber GetProductionRate()
    {
        BigNumber totalProduction = BigNumber.Zero;

        foreach (var generator in _generatorDatabase.GeneratorDictionary.Values)
        {
            totalProduction += generator.GetProductionRate();
        }

        _currencyData.SetTotalProduction(totalProduction);

        foreach (var generator in _generatorDatabase.GeneratorDictionary.Values)
        {
            generator.CalculatePercentage(totalProduction);
        }

        return totalProduction;
    }
}
