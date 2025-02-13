using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CurrencyDataSO", menuName = "Scriptable Objects/CurrencyDataSO")]
public class CurrencyDataSO : ScriptableObject
{
    public BigNumber LifeTimeCurrency;
    public BigNumber TotalCurrency;
    public BigNumber TotalProduction;

    private double _productionMultiplier = 1;
    private double _gemTotalMultiplier = 1;
    private double _collectedValueMultiplier = 1;

    public double ProductionMultiplier => _productionMultiplier;
    public double GemTotalMultiplier => _gemTotalMultiplier;
    public double CollectedValueMultiplier => _collectedValueMultiplier;


    public void SetProductionMultiplier()
    {
        _productionMultiplier = Math.Clamp(_productionMultiplier, 1, 10);
    }

    public void SetGemMultiplier()
    {
        _gemTotalMultiplier = Math.Clamp(_gemTotalMultiplier, 1, 20);
    }

    public void SetCollectedValueMultiplier()
    {
        _collectedValueMultiplier = Math.Clamp(_collectedValueMultiplier, 1, 20);
    }

}
