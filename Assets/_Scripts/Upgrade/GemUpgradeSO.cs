using UnityEngine;

[CreateAssetMenu(fileName = "GemUpgradeSO", menuName = "Scriptable Objects/Upgrade/GemUpgradeSO")]
public class GemUpgradeSO : BaseUpgradeSO
{
    [SerializeField] private GeneratorSO _generator;
    [SerializeField] private int _amountRequired;

    protected override bool CheckIfMeetRequirementToUnlock(double currency)
    {
        if (_generator == null)
        {
            Debug.LogError($"{name} has no gem generator attached. please fix");
            return false;
        }

        if (_generator.AmountOwned < _amountRequired)
        {
            return false;
        }

        if (currency < _cost.Value)
        {
            return false;
        }

        return true;
    }
}
