using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
[CreateAssetMenu(fileName = "UpgradeSO", menuName = "Scriptable Objects/UpgradeSO")]
public class UpgradeSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private FloatVariableSO _cost;
    [SerializeField] private BoolVariableSO _requiredSystem;
    [SerializeField] private FloatVariableSO _variableToModify;
    [SerializeField] private FloatModifier _modifierToApply;
    private bool _isDirty;

    public string Name => _name;
    public string Description => _description;
    public FloatVariableSO Cost => _cost;
    public double CostRequirement { get => Cost.Value * 0.33f; }
    public bool IsUnlocked { get; internal set; }
    public bool IsAlreadyBought { get; internal set; }

    internal void BuyUpgrade(double currency)
    {
        if (currency >= Cost.Value)
        {
            _variableToModify.AddModifier(_modifierToApply);
            IsAlreadyBought = true;
        }
    }

    public void UnlockUpgrade(double currency)
    {
        if (CheckIfMeetRequirementToUnlock(currency))
        {
            IsUnlocked = true;
        }
    }
    private bool CheckIfMeetRequirementToUnlock(double currency)
    {
        if (_requiredSystem == null)
        {
            if (currency >= CostRequirement)
            {
                return true;
            }
        }
        else
        {
            if (currency >= CostRequirement && _requiredSystem.Value)
            {
                return true;
            }
        }

        return false;
    }

}
