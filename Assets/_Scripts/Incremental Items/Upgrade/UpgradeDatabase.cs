using System.Collections.Generic;
using UnityEngine;

public class UpgradeDatabase
{
    static Dictionary<string, BaseUpgradeSO> _itemDictionary;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void Initialize()
    {
        _itemDictionary = new Dictionary<string, BaseUpgradeSO>();

        var allItems = Resources.LoadAll<BaseUpgradeSO>("");
        foreach (var item in allItems)
        {
            _itemDictionary.Add(item.Guid, item);
        }
    }

    public static List<BaseUpgradeSO> GetAllAssets()
    {
        List<BaseUpgradeSO> items = new();

        foreach (var item in _itemDictionary)
        {
            items.Add(item.Value);
        }

        return items;
    }

    public static BaseUpgradeSO GetAssetById(string id)
    {
        try
        {
            return _itemDictionary[id];
        }
        catch
        {
            Debug.LogError($"Cannot find {nameof(BaseUpgradeSO)} with id {id}");
            return null;
        }
    }
}
