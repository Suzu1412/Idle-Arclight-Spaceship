using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSystemSO", menuName = "Scriptable Objects/Incremental/Upgrade/UpgradeSystemSO")]
public class UpgradeSystemSO : BaseUpgradeSO
{
    [SerializeField] private BoolVariableSO _systemRequired;

    protected override bool CheckIfMeetRequirementToUnlock(double currency)
    {
        if (_systemRequired == null)
        {
            Debug.LogError($"{name} has no system required attached. please fix");
            return false;
        }

        if (!_systemRequired.Value)
        {
            return false;
        }

        if (currency < _cost)
        {
            return false;
        }

        return true;
    }
}
