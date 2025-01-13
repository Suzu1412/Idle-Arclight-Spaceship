using UnityEngine;

public class SerializableScriptableObject : ScriptableObject
{
    [SerializeField, ReadOnly] private string _guid;
    public string Guid => _guid;

    void OnEnable()
    {
        if (string.IsNullOrEmpty(Guid))
            GenerateGuid();
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(Guid))
            GenerateGuid();
    }

    [ContextMenu("Generate Guid")]
    internal void GenerateGuid()
    {
        _guid = System.Guid.NewGuid().ToString();
    }
}