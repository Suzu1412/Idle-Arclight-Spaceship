using UnityEngine;

public class GeneratorBuilder
{
    private double _baseCost;
    private double _productionBase;
    private int _amountOwned;
    private double _priceGrowthRate;

    public GeneratorBuilder Init(double baseCost, double productionBase, int amountOwned, double priceGrowthRate)
    {
        _baseCost = baseCost;
        _productionBase = productionBase;
        _amountOwned = amountOwned;
        _priceGrowthRate = priceGrowthRate;
        return this;
    }

    public GeneratorBuilder SetBaseCost(double baseCost)
    {
        _baseCost = baseCost;
        return this;
    }

    public GeneratorBuilder SetProductionBase(double productionBase)
    {
        _productionBase = productionBase;
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
        generatorSO.ProductionBase = _productionBase;
        generatorSO.AmountOwned = _amountOwned;
        generatorSO.PriceGrowthRate = _priceGrowthRate;

        return generatorSO;
    }

    public static implicit operator GeneratorSO(GeneratorBuilder builder)
    {
        return builder.Build();
    }
}
