using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GemUpgradeSO", menuName = "Scriptable Objects/Incremental/Upgrade/GemUpgradeSO")]
public class GemUpgradeSO : BaseUpgradeSO
{
    [Header("Modifiers")]
    [SerializeField] private GeneratorSO _generator;
    [SerializeField] private int _amountRequired;
    [SerializeField][Range(0f, 0.2f)] private float _percentageIncrease = 0.15f; // 15%

    public string GeneratorName => _generator.Name;

    protected override void OnEnable()
    {
        base.OnEnable();

        CalculateUpgradeCost();
    }

    internal override void BuyUpgrade(double currency)
    {
        if (currency >= Cost.Value)
        {
            ApplyUpgrade(true);
        }
    }

    protected override bool CheckIfMeetRequirementToUnlock(double currency)
    {
        if (_generator == null)
        {
            Debug.LogError($"{nameof(UpgradeSO)} has no gem generator attached. please fix");
            return false;
        }

        if (_generator.AmountOwned < _amountRequired)
        {
            return false;
        }

        return true;
    }

    internal override void ApplyUpgrade(bool val)
    {
        if (val)
        {
            _generator.AddModifier(_modifierToApply);
            _isApplied = true;
            OnProductionChangedEvent.RaiseEvent(this);
        }
        else
        {
            _generator.RemoveModifier(_modifierToApply);
            _isApplied = false;
        }
    }

    internal override void RemoveUpgrade()
    {
        _generator.RemoveModifier(_modifierToApply);
    }

    [ContextMenu("Calculate Upgrade cost")]
    private void CalculateUpgradeCost()
    {
        // Calculate the price of the Nth generator
        //double generatorPrice = _generator.BaseCost * Math.Pow(_generator.PriceGrowthRate, _amountRequired);

        // Add the percentage increase
        //double upgradeCost = generatorPrice * (1 + _percentageIncrease);

        // Determine the dynamic rounding factor
        //double roundingFactor = Math.Floor(Math.Pow(10, Math.Floor(Math.Log10(upgradeCost)) - 1));

        // Round down to the nearest multiple of the rounding factor
        //double roundedUpgradeCost = Math.Floor(upgradeCost / roundingFactor) * roundingFactor;

        //_cost = roundedUpgradeCost;
    }
}
