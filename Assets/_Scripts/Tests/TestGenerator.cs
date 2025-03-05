using System.Collections;
using System.Runtime.Remoting.Channels;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// https://github.com/BoundfoxStudios/fluentassertions-unity
public class TestGenerator
{
    private GeneratorSO _generator;
    private BigNumber baseCost = new(5, 0);
    private BigNumber production = new(0.1, 0);
    private int amountOwned = 0;
    private BigNumber priceGrowthRate = new(1.15, 0);


    [SetUp]
    public void SetupRequiredComponents()
    {
        _generator = A.Generator.Init(baseCost, production, amountOwned, priceGrowthRate);
    }

    [Test]
    public void TestSinglePrice()
    {
        Debug.Log(_generator._baseCost);
        _generator.GetTotalCostForGenerators(1).Should().Be(baseCost, "It should keep Base value");
    }

    [Test]
    [TestCase(1, 7, Description = "Price should be around 7")]
    [TestCase(10, 20, Description = "Price should be around 20")]
    [TestCase(50, 5418, Description = "Price should be around 5418")]
    public void TestPriceCalculator(int amountOwned, double finalValue)
    {
        _generator.AmountOwned = amountOwned;
        _generator.GetTotalCostForGenerators(1).ToDouble().Should().BeApproximately(finalValue, 2);
    }



    [Test]
    [TestCase(0, 0, Description = "Generation should be around 0")]
    [TestCase(10, 1, Description = "Generation should be around 1")]
    [TestCase(48, 4.8, Description = "Generation should be around 4.8")]
    public void TestGenerationRate(int amountOwned, double generationRate)
    {
        // TODO: Add new Test With Multipliers Unlocked
        //_generator.HasProductionChanged = true;
        //_generator.AmountOwned = amountOwned;
        //_generator.GetProductionRate().ToDouble().Should().BeApproximately(generationRate, 2);
    }


    [Test]
    [TestCase(310, 16, Description = "With 310 you should be able to buy 16")]
    [TestCase(108289, 57, Description = "With 108289 you should be able to buy 57")]
    [TestCase(100, 9, Description = "With 100 you should be able to buy 9")]
    public void TestMaxAmountYouCanBuy(double currency, int amountToBuy)
    {
        BigNumber bigNumberCurrency = new BigNumber(currency);
        _generator.GetMaxGenerators(bigNumberCurrency).Should().Be(amountToBuy);
    }

    /*
    [Test]
    [TestCase(1, 15, Description = "The Price of Buying 1 should be 15")]
    [TestCase(5, 103, Description = "The Price of Buying 5 should be 103")]
    [TestCase(30, 6535, Description = "The Price of Buying 50 should be 108289")]

    public void TestBulkPrice(int amountDesired, double price)
    {
        _generator.GetBulkCost(amountDesired).Should().BeApproximately(price, 2);
    }
    */

    // Objectives
    // 5- check bulk price for 10 - 100 -150
}
