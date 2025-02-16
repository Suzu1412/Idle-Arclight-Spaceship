using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Incremental/Generator/Generator Database")]
public class GeneratorDatabaseSO : ScriptableObject
{
    [SerializeField] private List<GeneratorSO> generatorList = new List<GeneratorSO>();
    private Dictionary<int, GeneratorSO> generatorDictionary;

    public IReadOnlyDictionary<int, GeneratorSO> GeneratorDictionary => generatorDictionary;

    public void InitializeDictionary()
    {
        generatorDictionary = new Dictionary<int, GeneratorSO>();

        for (int i = 0; i < generatorList.Count; i++)
        {
            generatorDictionary[i] = generatorList[i];
        }
    }

    public GeneratorSO Find(string guid)
    {
        return generatorList.Find(x => x.Guid == guid);
    }

    public GeneratorSO FindName(string name)
    {
        return generatorList.Find(x => x.name == name);
    }

#if UNITY_EDITOR

    [ContextMenu("Load All")]
    private void LoadAll()
    {
        generatorList = ScriptableObjectUtilities.FindAllScriptableObjectsOfType<GeneratorSO>("t:GeneratorSO", "Assets/_Data/Systems/Incremental Scriptable Objects/Generators");
    }

    private void OnValidate()
    {
        var allUnique = generatorList.GroupBy(x => x.Guid).All(g => g.Count() == 1);

        if (!allUnique)
        {
            foreach (var generator in generatorList)
            {
                generator.GenerateGuid();
            }
        }
    }
#endif
}
