using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SerializableScriptableObject : ScriptableObject
{
    [SerializeField, ReadOnly] private string _guid;
    public string Guid => _guid;

#if UNITY_EDITOR
    void OnEnable()
    {
        GenerateGuid();
    }

    [ContextMenu("Generate Guid")]
    internal void GenerateGuid()
    {
        var path = AssetDatabase.GetAssetPath(this);
        _guid = AssetDatabase.AssetPathToGUID(path);
    }
#endif
}