using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSO", menuName = "Scriptable Objects/Incremental/Upgrade/UpgradeSO")]
public class UpgradeSO : BaseUpgradeSO
{
    protected override bool CheckIfMeetRequirementToUnlock(double currency)
    {
        if (currency < _cost.Value)
        {
            return false;
        }

        return true;
    }
}
