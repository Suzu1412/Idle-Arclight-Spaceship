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

    public string Name => _name;
    public Sprite Image => _image;
    public double BaseCost { get => _baseCost; internal set => _baseCost = value; }
    public double ProductionBase { get => _productionBase; internal set => _productionBase = value; }
    public int AmountOwned { get => _amountOwned; internal set => _amountOwned = value; }
    public double PriceGrowthRate { get => _priceGrowthRate; internal set => _priceGrowthRate = value; }

    public void AddAmount(int amount)
    {
        _amountOwned += amount;
    }

    internal void SetAmount(int amount)
    {
        _amountOwned = amount;
    }

    public double GetProductionRate()
    {
        return _productionBase * _amountOwned; // * multipliers
    }

    public double GetNextCost()
    {
        return _baseCost * Math.Pow(_priceGrowthRate, _amountOwned);
    }

    public double GetBulkCost(int amountTobuy)
    {
        double bulkPrice = 0;

        for (int i = 0; i < amountTobuy; i++)
        {
            bulkPrice += CalculateNextCost(i);
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
            if (currencyLeft > CalculateNextCost(amountToBuy))
            {
                currencyLeft -= CalculateNextCost(amountToBuy);
                amountToBuy++;
            }
            else
            {
                hasEnoughCurrency = false;
            }
        }

        return amountToBuy;
    }

    private double CalculateNextCost(int addAmount)
    {
        return _baseCost * Math.Pow(_priceGrowthRate, _amountOwned + addAmount);
    }
}
