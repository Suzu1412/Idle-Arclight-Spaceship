using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

[System.Serializable]
public class Stat
{
    [SerializeField][ReadOnly] private StatComponentSO _statComponent;
    [SerializeField][ReadOnly] private float _baseValue;
    [SerializeField][ReadOnly] private StatConfig _config;
    [SerializeField][ReadOnly][Tooltip("Value is calculated by Base Value and Modifiers")] private float _value;
    private bool _isDirty = true;

    public StatComponentSO StatComponent { get => _statComponent; internal set => _statComponent = value; }
    public float BaseValue { get => _baseValue; internal set => _baseValue = value; }
    [SerializeField] private List<StatModifier> _modifiers = new();
    internal List<StatModifier> Modifiers => _modifiers;

    public Stat(StatConfig config)
    {
        _statComponent = config.StatComponent;
        _config = config;
        _modifiers = new();
    }

    public float Value
    {
        get
        {
            //if (_isDirty)
            //{
            //    CalculateValue();    
            //    _isDirty = false;
            //}
            CalculateValue();
            return _value;
        }
    }

    internal void CalculateValue()
    {
        _baseValue = Mathf.Clamp(_config.BaseValue, _statComponent.MinValue, _statComponent.MaxValue);
        float sumPercentAdditive = 0f;
        float finalValue = _baseValue;

        for (int i = 0; i < _modifiers.Count; i++)
        {
            switch (_modifiers[i].ModifierType)
            {
                case ModifierType.Flat:
                    finalValue += _modifiers[i].Value;
                    break;

                case ModifierType.PercentAdditive:
                    sumPercentAdditive += _modifiers[i].Value;
                    if (i + 1 >= _modifiers.Count || _modifiers[i + 1].ModifierType != ModifierType.PercentAdditive)
                    {
                        finalValue *= 1 + sumPercentAdditive;
                    }
                    break;

                case ModifierType.PercentMultiplicative:
                    finalValue *= 1 + _modifiers[i].Value;
                    break;
            }
        }

        _value = Mathf.Clamp(finalValue, StatComponent.MinValue, StatComponent.MaxValue);
    }

    internal void AddModifier(StatModifier modifier)
    {
        _isDirty = true;
        _modifiers.Add(modifier);
        _modifiers.Sort(CompareModifierType);
    }

    internal void RemoveModifier(StatModifier modifier)
    {
        _isDirty = true;
        _modifiers.Remove(modifier);
    }

    internal void RemoveAllModifiers()
    {
        _isDirty = true;
        _modifiers.Clear();
    }

    private int CompareModifierType(StatModifier x, StatModifier y)
    {
        if (x.ModifierType > y.ModifierType) return 1;
        if (x.ModifierType < y.ModifierType) return -1;
        return 0;
    }
}