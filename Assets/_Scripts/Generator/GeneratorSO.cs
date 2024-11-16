using UnityEngine;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
[CreateAssetMenu(fileName = "GeneratorSO", menuName = "Scriptable Objects/GeneratorSO")]
public class GeneratorSO : SerializableScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite[] _image;
    [SerializeField] private FloatVariableSO _baseCost;
    [SerializeField] private FloatVariableSO _production;
    [SerializeField] private FloatVariableSO _generatorProductionMultiplier;
    [SerializeField] private int _amountOwned;
    [SerializeField] private double _priceGrowthRate;
    [SerializeField] private double _totalProduction;
    private double _currentProduction;
    private bool _isDirty = true;

    public string Name => _name;
    // Image is assigned according to the amount of modifiers
    public Sprite Image => _image[_production.CountModifiers];
    public FloatVariableSO BaseCost { get => _baseCost; internal set => _baseCost = value; }
    public FloatVariableSO Production { get => _production; internal set => _production = value; }
    public int AmountOwned { get => _amountOwned; internal set => _amountOwned = value; }
    public double PriceGrowthRate { get => _priceGrowthRate; internal set => _priceGrowthRate = value; }
    public double TotalProduction { get => _totalProduction; internal set => _totalProduction = value; }
    public double CostRequirement { get => BaseCost.Value * 0.5; }
    public double BulkCost { get; private set; }
    public bool IsUnlocked { get; internal set; }
    public FormattedNumber CostFormatted { get; private set; }
    public FormattedNumber ProductionFormatted { get; private set; }

    public void CheckIfMeetRequirementsToUnlock(double currency)
    {
        if (currency >= CostRequirement)
        {
            IsUnlocked = true;
        }
    }

    public void AddAmount(int amount)
    {
        _isDirty = true;
        _amountOwned += amount;
    }

    public void CalculateProductionRate()
    {
        _currentProduction = Math.Round(_production.Value * _generatorProductionMultiplier.Value * _amountOwned, 1);
        ProductionFormatted = FormatNumber.FormatDouble(_currentProduction, ProductionFormatted);
        _isDirty = false;
    }

    public double GetProductionRate()
    {
        if (_isDirty)
        {
            CalculateProductionRate();
        }
        TotalProduction = Math.Round(TotalProduction + _currentProduction, 1);
        return _currentProduction;
    }

    public double GetBulkCost(int amountTobuy)
    {
        BulkCost = 0;

        for (int i = 0; i < amountTobuy; i++)
        {
            BulkCost += GetNextCost(i);
        }

        CostFormatted = FormatNumber.FormatDouble(BulkCost, CostFormatted);
        return BulkCost;
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
        return Math.Ceiling(BaseCost.Value * Math.Pow(_priceGrowthRate, _amountOwned + addAmount));
    }
}
