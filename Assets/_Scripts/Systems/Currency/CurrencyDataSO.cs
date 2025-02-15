using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CurrencyDataSO", menuName = "Scriptable Objects/CurrencyDataSO")]
public class CurrencyDataSO : ScriptableObject
{
    public BigNumber LifeTimeCurrency;
    public BigNumber TotalCurrency;
    public BigNumber TotalProduction;

    private BigNumber _currentProduction;

    private double _productionMultiplier = 1; // Event multiplier
    private double _gemTotalMultiplier = 1; // Event multiplier
    private double _collectedValueMultiplier = 1; // Event multiplier


    public BigNumber CurrentProduction => _currentProduction;
    public double ProductionMultiplier => _productionMultiplier;
    public double GemTotalMultiplier => _gemTotalMultiplier;
    public double CollectedValueMultiplier => _collectedValueMultiplier;


    public void SetProductionMultiplier(double productionMultiplier)
    {
        _productionMultiplier = Math.Clamp(productionMultiplier, 1, 10);
    }

    public void SetGemMultiplier(double gemTotalMultiplier)
    {
        _gemTotalMultiplier = Math.Clamp(gemTotalMultiplier, 1, 20);
    }

    public void SetCollectedValueMultiplier(double collectedValueMultiplier)
    {
        _collectedValueMultiplier = Math.Clamp(collectedValueMultiplier, 1, 20);
    }


    public BigNumber GetGemOnGetValue(int amount)
    {
        BigNumber totalAmount = new(amount);

        totalAmount *= CollectedValueMultiplier * GemTotalMultiplier; // Calculate Events Multipliers
        //totalAmount += CurrentProduction *  

        return totalAmount;
            //double amount = _amount;
        //amount *= _crystalOnGetMultiplier.Value * _crystalTotalMultiplier.Value + (_currentProduction.Value * _productionPercentage.Value);
        //amount += amount * (_gemTotalAmount.Value * _gemTotalAmountMultiplier.Value);

    }
}
