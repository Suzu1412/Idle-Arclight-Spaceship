using System.Collections.Generic;
using UnityEngine;

public class UpgradeDatabase
{
    static Dictionary<string, UpgradeSO> _itemDictionary;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void Initialize()
    {
        _itemDictionary = new Dictionary<string, UpgradeSO>();

        var allItems = Resources.LoadAll<UpgradeSO>("");
        foreach (var item in allItems)
        {
            _itemDictionary.Add(item.Guid, item);
        }
    }

    public static List<UpgradeSO> GetAllAssets()
    {
        List<UpgradeSO> items = new();

        foreach (var item in _itemDictionary)
        {
            items.Add(item.Value);
        }

        return items;
    }

    public static UpgradeSO GetAssetById(string id)
    {
        try
        {
            return _itemDictionary[id];
        }
        catch
        {
            Debug.LogError($"Cannot find {nameof(UpgradeSO)} with id {id}");
            return null;
        }
    }
}
