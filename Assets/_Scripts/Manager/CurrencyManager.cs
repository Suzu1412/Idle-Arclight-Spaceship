using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AddSaveDataRunTimeSet))]
public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
{
    [SerializeField] private List<GeneratorSO> _generators;
    [SerializeField] private DoubleGameEventListener _gainCurrencyListener;
    [SerializeField] private DoubleGameEvent _totalCurrency;
    [Header("Save Data")]
    [SerializeField] private CurrencyData _currencyData;
    [SerializeField] private float _delayToGenerate = 1f;
    [field: SerializeField] public SerializableGuid Id { get; set; }

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
        _totalCurrency.RaiseEvent(_currencyData.TotalCurrency);
    }

    public void SaveData(GameData gameData)
    {
        gameData.CurrencyData = _currencyData;
    }

    public void LoadData(GameData gameData)
    {
        _currencyData.TotalCurrency = gameData.CurrencyData.TotalCurrency;
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
                amount += generator.GenerationRate * generator.CurrentAmount;
            }
            
            Increment(amount);
        }
    }
}
