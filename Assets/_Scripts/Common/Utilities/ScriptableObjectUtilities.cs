#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ScriptableObjectUtilities
{
    public static List<T> FindAllScriptableObjectsOfType<T>(string filter, string folder = "Assets")
        where T : ScriptableObject
    {
        return AssetDatabase.FindAssets(filter, new[] { folder })
            .Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
            .Cast<T>().ToList();
    }

    // Use example
    // _unlockedSystems = ScriptableObjectUtilities.FindAllScriptableObjectsOfType<UnlockedSystemSO>("t:UnlockedSystemSO", "Assets/_Data/Incremental Scriptable Objects/Unlocked Systems");
}
#endif