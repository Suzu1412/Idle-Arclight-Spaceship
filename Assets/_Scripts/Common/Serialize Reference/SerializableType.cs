using System;
using UnityEngine;

[System.Serializable]
public class SerializableType : ISerializationCallbackReceiver
{
    [SerializeField] private string _assemblyQualifiedName = string.Empty;
    
    public Type Type { get; private set; }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        _assemblyQualifiedName = Type?.AssemblyQualifiedName ?? _assemblyQualifiedName;

    }
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (!TryGetType(_assemblyQualifiedName, out var type))
        {
            Debug.LogError($"Type: {_assemblyQualifiedName} not found");
            return;
        }
        Type = type;
    }

    static bool TryGetType(string typeString, out Type type)
    {
        type = Type.GetType(typeString);
        return type != null || !string.IsNullOrEmpty(typeString);
    }
}
