using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListUpgradeSO", menuName = "Scriptable Objects/Incremental/Upgrade/ListUpgradeSO")]
public class ListUpgradeSO : ScriptableObject
{
    [SerializeField] private List<BaseUpgradeSO> _upgrades;

    public List<BaseUpgradeSO> Upgrades => _upgrades;

    public BaseUpgradeSO Find(string guid)
    {
        return _upgrades.Find(x => x.Guid == guid);
    }

    [ContextMenu("Load All")]
    private void LoadAll()
    {
        _upgrades = ScriptableObjectUtilities.FindAllScriptableObjectsOfType<BaseUpgradeSO>("t:BaseUpgradeSO", "Assets/_Scripts/Incremental Items/Upgrade");
    }
}
