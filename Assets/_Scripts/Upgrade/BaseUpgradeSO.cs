using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
public abstract class BaseUpgradeSO : SerializableScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField] protected double _baseCost;
    [SerializeField] protected double _costRequirement;
    protected double _cost;
    protected bool _isDirty;


    public string Name => _name;
    public double Cost
    {
        get
        {
            if (_isDirty)
            {

            }
            return _cost;
        }
    }

    public bool IsUnlocked { get; internal set; }

    public abstract void ApplyUpgrade();

    protected abstract bool CheckIfMeetRequirementToUnlock();

    public void UnlockUpgrade()
    {
        if (CheckIfMeetRequirementToUnlock())
        {
            IsUnlocked = true;
        }
    }
}
