using UnityEngine;

[CreateAssetMenu(fileName = "GemUpgradeSO", menuName = "Scriptable Objects/Incremental/Upgrade/GemUpgradeSO")]
public class GemUpgradeSO : BaseUpgradeSO
{
    [Header("Modifiers")]
    [SerializeField] private GeneratorSO _generator;
    [SerializeField] private int _amountRequired;

    public string GeneratorName => _generator.Name;

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
            OnProductionChangedEvent.RaiseEvent();
        }
    }

    internal override void RemoveUpgrade()
    {
        _generator.RemoveModifier(_modifierToApply);
    }
}
