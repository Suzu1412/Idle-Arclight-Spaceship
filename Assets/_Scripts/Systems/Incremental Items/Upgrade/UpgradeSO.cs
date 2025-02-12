using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSO", menuName = "Scriptable Objects/Incremental/Upgrade/UpgradeSO")]
public class UpgradeSO : BaseUpgradeSO
{
    [SerializeField] private double _costRequirement;
    [Header("Modifiers")]
    [SerializeField] protected FloatVariableSO _variableToModify;

    protected override bool CheckIfMeetRequirementToUnlock(double currency)
    {
        if (currency < _costRequirement)
        {
            return false;
        }

        return true;
    }

    internal override void ApplyUpgrade(bool val)
    {
        if (val)
        {
            _variableToModify.AddModifier(_modifierToApply);
            _isApplied = true;
            OnProductionChangedEvent.RaiseEvent(this);
        }
        else
        {
            _variableToModify.RemoveModifier(_modifierToApply);
            _isApplied = false;
        }
    }

    internal override void RemoveUpgrade()
    {
        _variableToModify.RemoveModifier(_modifierToApply);

    }
}
