using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListUnlockedSO", menuName = "Scriptable Objects/Incremental/Unlocked System/ListUnlockedSO")]
public class ListUnlockedSystemsSO : ScriptableObject
{
    [SerializeField] private List<UnlockedSystemSO> _unlockedSystems;

    public List<UnlockedSystemSO> UnlockedSystems => _unlockedSystems;

    public UnlockedSystemSO Find(string guid)
    {
        return _unlockedSystems.Find(x => x.Guid == guid);
    }

    [ContextMenu("Load All")]
    private void LoadAll()
    {
#if UNITY_EDITOR

        _unlockedSystems = ScriptableObjectUtilities.FindAllScriptableObjectsOfType<UnlockedSystemSO>("t:UnlockedSystemSO", "Assets/_Data/Incremental Scriptable Objects/Unlocked Systems");
#endif
    }
}
