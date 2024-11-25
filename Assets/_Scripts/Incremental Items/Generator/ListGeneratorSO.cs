using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListGeneratorSO", menuName = "Scriptable Objects/Incremental/ListGeneratorSO")]
public class ListGeneratorSO : ScriptableObject
{
    [SerializeField] private List<GeneratorSO> _generators;

    public List<GeneratorSO> Generators => _generators;

    public GeneratorSO Find(string guid)
    {
        return _generators.Find(x => x.Guid == guid);
    }

}
