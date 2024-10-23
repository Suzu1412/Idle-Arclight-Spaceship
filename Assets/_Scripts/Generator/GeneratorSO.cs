using UnityEngine;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
[CreateAssetMenu(fileName = "GeneratorSO", menuName = "Scriptable Objects/GeneratorSO")]
public class GeneratorSO : SerializableScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _image;
    [SerializeField] private double _baseCost;
    [SerializeField] private double _productionBase;
    [SerializeField] private int _amountOwned;
    [SerializeField] private double _priceGrowthRate;
    [SerializeField] private double _totalProduction;
    private double _production;
    private bool _isDirty = true;

    public string Name => _name;
    public Sprite Image => _image;
    public double BaseCost { get => _baseCost; internal set => _baseCost = value; }
    public double ProductionBase { get => _productionBase; internal set => _productionBase = value; }
    public int AmountOwned { get => _amountOwned; internal set => _amountOwned = value; }
    public double PriceGrowthRate { get => _priceGrowthRate; internal set => _priceGrowthRate = value; }
    public double TotalProduction { get => _totalProduction; internal set => _totalProduction = value; }

    public void AddAmount(int amount)
    {
        _isDirty = true;
        _amountOwned += amount;
    }

    internal void SetAmount(int amount)
    {
        _isDirty = true;
        _amountOwned = amount;
    }

    public double GetProductionRate()
    {
        if (_isDirty)
        {
            _production = _productionBase * _amountOwned;
            _isDirty = false;
        }
        return _production;
    }

    public double GetBulkCost(int amountTobuy)
    {
        double bulkPrice = 0;

        for (int i = 0; i < amountTobuy; i++)
        {
            bulkPrice += GetNextCost(i);
        }

        return bulkPrice;
    }

    public int CalculateMaxAmountToBuy(double currency)
    {
        int amountToBuy = 0;
        double currencyLeft = currency;
        bool hasEnoughCurrency = true;

        while (hasEnoughCurrency)
        {
            if (currencyLeft >= GetNextCost(amountToBuy))
            {
                currencyLeft -= GetNextCost(amountToBuy);
                amountToBuy++;
            }
            else
            {
                hasEnoughCurrency = false;
            }
        }

        return amountToBuy;
    }

    public double GetNextCost(int addAmount = 0)
    {
        return Math.Ceiling(_baseCost * Math.Pow(_priceGrowthRate, _amountOwned + addAmount));
    }
}
