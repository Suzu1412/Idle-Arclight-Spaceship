using System.Collections.Generic;
using UnityEngine;

public class GeneratorDataBase
{
    static Dictionary<string, GeneratorSO> _itemDictionary;

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void Initialize()
    {
        _itemDictionary = new Dictionary<string, GeneratorSO>();

        var allItems = Resources.LoadAll<GeneratorSO>("");
        foreach (var item in allItems)
        {
            _itemDictionary.Add(item.Guid, item);
        }
    }

    public static List<GeneratorSO> GetAllAssets()
    {
        List<GeneratorSO> items = new();

        foreach (var item in _itemDictionary)
        {
            items.Add(item.Value);
        }

        return items;
    }

    public static GeneratorSO GetAssetById(string id)
    {
        try
        {
            return _itemDictionary[id];
        }
        catch
        {
            Debug.LogError($"Cannot find {nameof(GeneratorSO)} with id {id}");
            return null;
        }
    }
}
