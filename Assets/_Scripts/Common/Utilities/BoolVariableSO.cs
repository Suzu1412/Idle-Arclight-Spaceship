using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariableSO", menuName = "Scriptable Objects/Variable/BoolVariableSO")]
public class BoolVariableSO : ScriptableObject
{
    [SerializeField] private bool _value;

    public bool Value { get => _value; set => _value = value; }
}
