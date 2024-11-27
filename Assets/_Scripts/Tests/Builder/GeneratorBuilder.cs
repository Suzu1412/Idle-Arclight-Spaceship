using UnityEngine;

public class GeneratorBuilder
{
    private FloatVariableSO _baseCost;
    private FloatVariableSO _production;
    private int _amountOwned;
    private double _priceGrowthRate;

    public GeneratorBuilder Init(FloatVariableSO baseCost, FloatVariableSO production, int amountOwned, double priceGrowthRate)
    {
        _baseCost = baseCost;
        _production = production;
        _amountOwned = amountOwned;
        _priceGrowthRate = priceGrowthRate;
        return this;
    }

    public GeneratorBuilder SetBaseCost(FloatVariableSO baseCost)
    {
        _baseCost = baseCost;
        return this;
    }

    public GeneratorBuilder SetProductionBase(FloatVariableSO production)
    {
        _production = production;
        return this;
    }

    public GeneratorBuilder SetAmountOwned(int amountOwned)
    {
        _amountOwned = amountOwned;
        return this;
    }

    public GeneratorBuilder SetPriceGrowthRate(double priceGrowthRate)
    {
        _priceGrowthRate = priceGrowthRate;
        return this;
    }

    private GeneratorSO Build()
    {
        var generatorSO = ScriptableObject.CreateInstance<GeneratorSO>();
        generatorSO.BaseCost = _baseCost;
        generatorSO.Production = _production;
        generatorSO.AmountOwned = _amountOwned;
        generatorSO.PriceGrowthRate = _priceGrowthRate;

        return generatorSO;
    }

    public static implicit operator GeneratorSO(GeneratorBuilder builder)
    {
        return builder.Build();
    }
}
