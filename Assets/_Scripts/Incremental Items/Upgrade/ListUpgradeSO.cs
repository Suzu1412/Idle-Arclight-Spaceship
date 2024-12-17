using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ListUpgradeSO", menuName = "Scriptable Objects/Incremental/Upgrade/ListUpgradeSO")]
public class ListUpgradeSO : ScriptableObject
{
    [SerializeField] private List<BaseUpgradeSO> _upgrades;

    public List<BaseUpgradeSO> Upgrades => _upgrades;

    private void OnEnable()
    {
        //OrderByCost();
    }

    

    public BaseUpgradeSO Find(string guid)
    {
        return _upgrades.Find(x => x.Guid == guid);
    }

    [ContextMenu(itemName: "Order By Cost")]
    private void OrderByCost()
    {
        _upgrades = _upgrades.OrderBy(x => x.Cost.Value).ToList();
    }


#if UNITY_EDITOR

    [ContextMenu(itemName: "Load All")]
    private void LoadAll()
    {

        _upgrades = ScriptableObjectUtilities.FindAllScriptableObjectsOfType<BaseUpgradeSO>("t:BaseUpgradeSO", "Assets/_Data/Incremental Scriptable Objects/Upgrades");
    }

    private void OnValidate()
    {
        var allUnique = _upgrades.GroupBy(x => x.Guid).All(g => g.Count() == 1);

        if (!allUnique)
        {
            foreach (var upgrade in _upgrades)
            {
                upgrade.GenerateGuid();
            }
        }
    }
#endif

}
