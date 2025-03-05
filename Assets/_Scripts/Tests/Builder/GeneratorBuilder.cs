using UnityEngine;

public class GeneratorBuilder
{
    private BigNumber _baseCost;
    private BigNumber _baseProduction;
    private int _amountOwned;
    private BigNumber _priceGrowthRate;

    public GeneratorBuilder Init(BigNumber baseCost, BigNumber production, int amountOwned, BigNumber priceGrowthRate)
    {
        _baseCost = baseCost;
        _baseProduction = production;
        _amountOwned = amountOwned;
        _priceGrowthRate = priceGrowthRate;
        return this;
    }

    public GeneratorBuilder SetBaseCost(BigNumber baseCost)
    {
        _baseCost = baseCost;
        return this;
    }

    public GeneratorBuilder SetProductionBase(BigNumber production)
    {
        _baseProduction = production;
        return this;
    }

    public GeneratorBuilder SetAmountOwned(int amountOwned)
    {
        _amountOwned = amountOwned;
        return this;
    }

    public GeneratorBuilder SetPriceGrowthRate(BigNumber priceGrowthRate)
    {
        _priceGrowthRate = priceGrowthRate;
        return this;
    }

    private GeneratorSO Build()
    {
        var generatorSO = ScriptableObject.CreateInstance<GeneratorSO>();
        generatorSO._baseCost = _baseCost;
        generatorSO._baseProduction = _baseProduction;
        generatorSO._amountOwned = _amountOwned;
        generatorSO._priceGrowthRate = _priceGrowthRate;

        return generatorSO;
    }

    public static implicit operator GeneratorSO(GeneratorBuilder builder)
    {
        return builder.Build();
    }
}
