using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListUpgradeSO", menuName = "Scriptable Objects/Incremental/Upgrade/ListUpgradeSO")]
public class ListUpgradeSO : ScriptableObject
{
    [SerializeField] private List<BaseUpgradeSO> _upgrades;

    public List<BaseUpgradeSO> Upgrades => _upgrades;
}
