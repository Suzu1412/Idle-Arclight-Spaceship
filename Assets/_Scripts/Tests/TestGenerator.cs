using System.Collections;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// https://github.com/BoundfoxStudios/fluentassertions-unity
public class TestGenerator
{
    private GeneratorSO _generator;

    [OneTimeSetUp]
    public void SetupRequiredComponents()
    {
        _generator = A.Generator.Init(15, 0.1, 0, 1.17);
    }

    [Test]
    public void TestSinglePrice()
    {
        _generator.GetNextCost().Should().Be(15, "It should keep Base value");

    }

    // Objectives
    // 1- check price no amount owned
    // 2- check price after buying each one
    // 3- return generation (Later calculate Multipliers)
    // 4- check how many can buy with money
    // 5- check bulk price for 10 - 100 -150

}