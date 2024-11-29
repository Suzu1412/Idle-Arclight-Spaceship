using System.Collections.Generic;
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

    [ContextMenu("Load All")]
    private void LoadAll()
    {
#if UNITY_EDITOR
        _generators = ScriptableObjectUtilities.FindAllScriptableObjectsOfType<GeneratorSO>("t:GeneratorSO", "Assets/_Scripts/Incremental Items/Generator");
#endif
    }

}
