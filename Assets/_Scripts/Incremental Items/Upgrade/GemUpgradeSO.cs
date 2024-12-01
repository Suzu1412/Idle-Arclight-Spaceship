using UnityEngine;

[CreateAssetMenu(fileName = "GemUpgradeSO", menuName = "Scriptable Objects/Incremental/Upgrade/GemUpgradeSO")]
public class GemUpgradeSO : BaseUpgradeSO
{
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

        if (currency < _cost.Value)
        {
            return false;
        }

        return true;
    }
}
