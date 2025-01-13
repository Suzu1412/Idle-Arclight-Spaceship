using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ListGeneratorSO", menuName = "Scriptable Objects/Incremental/Generator/ListGeneratorSO")]
public class ListGeneratorSO : ScriptableObject
{
    [SerializeField] private List<GeneratorSO> _generators;

    public List<GeneratorSO> Generators => _generators;

    public GeneratorSO Find(string guid)
    {
        return _generators.Find(x => x.Guid == guid);
    }

    public GeneratorSO FindName(string name)
    {
        return _generators.Find(x => x.name == name);
    }

#if UNITY_EDITOR

    [ContextMenu("Load All")]
    private void LoadAll()
    {
        _generators = ScriptableObjectUtilities.FindAllScriptableObjectsOfType<GeneratorSO>("t:GeneratorSO", "Assets/_Data/Incremental Scriptable Objects/Generators");
    }

    private void OnValidate()
    {
        var allUnique = _generators.GroupBy(x => x.Guid).All(g => g.Count() == 1);

        if (!allUnique)
        {
            foreach (var generator in _generators)
            {
                generator.GenerateGuid();
            }
        }
    }
#endif
}
