using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatModifier
{
    [SerializeField] private ModifierType _modifierType;
    [SerializeField] private StatComponentSO _statComponent;
    [SerializeField][Tooltip("Multiplicative must be greater than 1 to increase stats")] private float _value = 1.0f;
    [SerializeField] private string _source;

    public ModifierType ModifierType { get => _modifierType; internal set => _modifierType = value; }
    public StatComponentSO StatComponent { get => _statComponent; internal set => _statComponent = value; }
    public float Value { get => _value; internal set => _value = value; }
    public string Source { get => _source; internal set => _source = value; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modifierType">Type of Modifier</param>
    /// <param name="statType">Stat Type</param>
    /// <param name="value">Value: 1.0 or more for multiplicative</param>
    public StatModifier(ModifierType modifierType, StatComponentSO statComponent, float value, string source)
    {
        _modifierType = modifierType;
        _statComponent = StatComponent;
        _value = value;
        _source = source;
    }
}
