using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
public abstract class BaseUpgradeSO : SerializableScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;
    [SerializeField] protected FloatVariableSO _cost;
    [SerializeField] protected FloatVariableSO _variableToModify;
    [SerializeField] protected FloatModifier _modifierToApply;
    protected FormattedNumber CostFormatted { get; private set; }

    protected bool _isDirty;

    public string Name => _name;
    public string Description => _description;
    public FloatVariableSO Cost => _cost;
    public double CostRequirement { get => Cost.Value * 0.5f; }
    public bool IsRequirementMet { get; internal set; }
    public bool IsApplied { get; protected set; }

    private void OnDisable()
    {
        _variableToModify.RemoveModifier(_modifierToApply);
        IsApplied = false;
    }

    internal virtual void BuyUpgrade(double currency)
    {
        if (currency >= Cost.Value)
        {
            ApplyUpgrade(true);
        }
    }

    public void UnlockUpgradeInStore(double currency)
    {
        if (CheckIfMeetRequirementToUnlock(currency))
        {
            IsRequirementMet = true;
        }
    }

    internal void ApplyUpgrade(bool val)
    {
        if (val)
        {
            _variableToModify.AddModifier(_modifierToApply);
            IsApplied = true;
        }
    }

    public FormattedNumber GetCost()
    {
        CostFormatted = FormatNumber.FormatDouble(Cost.Value, CostFormatted);
        return CostFormatted;
    }

    protected abstract bool CheckIfMeetRequirementToUnlock(double currency);
}
