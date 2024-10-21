using System.Collections.Generic;
using UnityEngine;

public class GeneratorDataBase
{
    static Dictionary<string, GeneratorSO> _generatorDictionary;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void Initialize()
    {
        _generatorDictionary = new Dictionary<string, GeneratorSO>();

        var _generatorDetails = Resources.LoadAll<GeneratorSO>("");
        foreach (var _generator in _generatorDetails)
        {
            _generatorDictionary.Add(_generator.Guid, _generator);
        }
    }

    public static List<GeneratorSO> GetAllAssets()
    {
        List<GeneratorSO> generators = new();
        
        foreach(var generator in _generatorDictionary)
        {
            generators.Add(generator.Value);
        }
        
        return generators;
    }

    public static GeneratorSO GetAssetById(string id)
    {
        try
        {
            return _generatorDictionary[id];
        }
        catch
        {
            Debug.LogError($"Cannot find _generator details with id {id}");
            return null;
        }
    }
}
