using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
public abstract class BaseUpgradeSO : SerializableScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;
    [SerializeField] protected double _cost;
    [SerializeField] protected double _costRequirement;
    [SerializeField] protected FloatVariableSO _upgradeCostMultiplier = default;
    [SerializeField] protected FloatVariableSO _variableToModify;
    [SerializeField] protected FloatModifier _modifierToApply;
    [SerializeField] protected bool _isApplied;
    [SerializeField] protected VoidGameEvent OnProductionChangedEvent;
    protected DoubleVariableSO _finalCost;

    protected FormattedNumber CostFormatted { get; private set; }
    protected bool _isDirty;

    public string Name => _name;
    public string Description => _description;
    public double CostRequirement => _costRequirement;
    public bool IsRequirementMet { get; internal set; }
    public bool IsApplied => _isApplied;

    public DoubleVariableSO Cost
    {
        get
        {
            _finalCost.Value = _cost * _upgradeCostMultiplier.Value;
            return _finalCost;
        }
    }

    private void OnEnable()
    {
        _finalCost = ScriptableObject.CreateInstance<DoubleVariableSO>();
        _finalCost.Initialize(0, 0, double.MaxValue);
        _modifierToApply.SetSource(name);
    }


    private void OnDisable()
    {
        _variableToModify.RemoveModifier(_modifierToApply);
        _isApplied = false;
    }

    internal void Initialize()
    {
        IsRequirementMet = false;
        ApplyUpgrade(false);
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
            _isApplied = true;
            OnProductionChangedEvent.RaiseEvent();
        }
    }

    public FormattedNumber GetCost()
    {
        CostFormatted = FormatNumber.FormatDouble(Cost.Value, CostFormatted);
        return CostFormatted;
    }

    protected abstract bool CheckIfMeetRequirementToUnlock(double currency);
}
