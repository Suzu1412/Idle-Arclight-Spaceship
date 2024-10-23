using System.Collections;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// https://github.com/BoundfoxStudios/fluentassertions-unity
public class TestGenerator
{
    private GeneratorSO _generator;

    [SetUp]
    public void SetupRequiredComponents()
    {
        // Test Values are compared to the ones of Cookie Clicker - Clicker
        _generator = A.Generator.Init(15, 0.1, 0, 1.15);
    }

    [Test]
    public void TestSinglePrice()
    {
        _generator.GetNextCost().Should().Be(15, "It should keep Base value");
    }

    [Test]
    [TestCase(1, 18, Description = "Price should be around 18")]
    [TestCase(10, 61, Description = "Price should be around 61")]
    [TestCase(50, 16255, Description = "Price should be around 16255")]
    public void TestPriceCalculator(int amountOwned, double finalValue)
    {
        _generator.AmountOwned = amountOwned;
        _generator.GetNextCost().Should().BeApproximately(finalValue, 2);
    }

    [Test]
    [TestCase(0, 0, Description = "Generation should be around 0")]
    [TestCase(10, 1, Description = "Generation should be around 10")]
    [TestCase(48, 4.8, Description = "Generation should be around 4.8")]
    public void TestGenerationRate(int amountOwned, double generationRate)
    {
        // TODO: Add new Test With Multipliers Unlocked
        _generator.AmountOwned = amountOwned;
        _generator.GetProductionRate().Should().BeApproximately(generationRate, 2);
    }

    [Test]
    [TestCase(310, 10, Description = "With 310 you should be able to buy 10")]
    [TestCase(108289, 50, Description = "With 108289 you should be able to buy 50")]
    [TestCase(112000, 50, Description = "With 112000 you should be able to buy 50")]
    public void TestMaxAmountYouCanBuy(double currency, int amountToBuy)
    {
        _generator.CalculateMaxAmountToBuy(currency).Should().Be(amountToBuy);
    }

    [Test]
    [TestCase(1, 15, Description = "The Price of Buying 1 should be 15")]
    [TestCase(5, 103, Description = "The Price of Buying 5 should be 103")]
    [TestCase(30, 6535, Description = "The Price of Buying 50 should be 108289")]

    public void TestBulkPrice(int amountDesired, double price)
    {
        _generator.GetBulkCost(amountDesired).Should().BeApproximately(price, 2);
    }

    // Objectives
    // 5- check bulk price for 10 - 100 -150

}