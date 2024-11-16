using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
public abstract class BaseUpgradeSO : SerializableScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;
    [SerializeField] protected FloatVariableSO _cost;
    [SerializeField] protected BoolVariableSO _requiredSystem;
    [SerializeField] protected FloatVariableSO _variableToModify;
    [SerializeField] protected FloatModifier _modifierToApply;
    protected bool _isDirty;

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
    protected abstract bool CheckIfMeetRequirementToUnlock(double currency);
}
