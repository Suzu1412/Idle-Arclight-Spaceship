using UnityEngine;

public abstract class BaseUpgradeSO : ScriptableObject
{
    [SerializeField] protected string _name;



    public string Name => _name;

    public abstract void ApplyUpgrade();

    public abstract bool Requirements();
}
