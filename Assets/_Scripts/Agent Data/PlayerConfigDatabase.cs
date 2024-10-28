using System.Collections.Generic;
using UnityEngine;

public class PlayerConfigDatabase
{
    static Dictionary<string, PlayerAgentDataSO> _itemDictionary;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void Initialize()
    {
        _itemDictionary = new Dictionary<string, PlayerAgentDataSO>();

        var allItems = Resources.LoadAll<PlayerAgentDataSO>("");
        foreach (var item in allItems)
        {
            _itemDictionary.Add(item.Guid, item);
        }
    }

    public static List<PlayerAgentDataSO> GetAllAssets()
    {
        List<PlayerAgentDataSO> items = new();

        foreach (var item in _itemDictionary)
        {
            items.Add(item.Value);
        }

        return items;
    }

    public static PlayerAgentDataSO GetAssetById(string id)
    {
        try
        {
            return _itemDictionary[id];
        }
        catch
        {
            Debug.LogError($"Cannot find {nameof(PlayerAgentDataSO)} with id {id}");
            return null;
        }
    }
}
