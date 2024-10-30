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

    private bool _isDirty = true;

    public string Name => _name;
    public Sprite Image => _image;
    public double BaseCost { get => _baseCost; internal set => _baseCost = value; }
    public double ProductionBase { get => _productionBase; internal set => _productionBase = value; }
    public int AmountOwned { get => _amountOwned; internal set => _amountOwned = value; }
    public double PriceGrowthRate { get => _priceGrowthRate; internal set => _priceGrowthRate = value; }
    public double TotalProduction { get => _totalProduction; internal set => _totalProduction = value; }
    public double Cost { get; private set; }
    public double Production { get; private set; }
    public FormattedNumber CostFormatted { get; private set; }
    public FormattedNumber ProductionFormatted { get; private set; }

    public void AddAmount(int amount)
    {
        _isDirty = true;
        _amountOwned += amount;
    }

    public void CalculateProductionRate()
    {
        Production = Math.Round(_productionBase * _amountOwned, 1);
        ProductionFormatted = FormatNumber.FormatDouble(Production, ProductionFormatted);
        _isDirty = false;
    }

    public double GetProductionRate()
    {
        if (_isDirty)
        {
            CalculateProductionRate();
        }
        TotalProduction = Math.Round(TotalProduction + Production, 1);
        return Production;
    }

    public double GetBulkCost(int amountTobuy)
    {
        double bulkPrice = 0;

        for (int i = 0; i < amountTobuy; i++)
        {
            bulkPrice += GetNextCost(i);
        }

        Cost = bulkPrice;
        CostFormatted = FormatNumber.FormatDouble(Cost, CostFormatted);
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

    internal void SetAmount(int amount)
    {
        _isDirty = true;
        _amountOwned = amount;
    }

    internal void SetTotalProduction(double totalProduction)
    {
        _totalProduction = totalProduction;
    }

    internal double GetNextCost(int addAmount = 0)
    {
        return Math.Ceiling(_baseCost * Math.Pow(_priceGrowthRate, _amountOwned + addAmount));
    }
}
