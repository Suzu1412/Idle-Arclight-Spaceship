using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariableSO", menuName = "Scriptable Objects/Variable/FloatVariableSO")]
public class FloatVariableSO : ScriptableObject
{
    [SerializeField] private float _baseValue;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    [SerializeField] private List<FloatModifier> _modifiers = new();
    [SerializeField] private float _value;

    public int CountModifiers => _modifiers.Count;

    private bool _isDirty = false;

    private void OnEnable()
    {
        RemoveAllModifiers();
        _isDirty = true;
        _baseValue = Mathf.Clamp(_baseValue, _minValue, _maxValue);
    }

    private void OnDisable()
    {
        RemoveAllModifiers();
    }

    private void OnValidate()
    {
        _isDirty = true;
        _baseValue = Mathf.Clamp(_baseValue, _minValue, _maxValue);
    }

    public void Initialize(float baseValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        _baseValue = Mathf.Clamp(baseValue, _minValue, _maxValue);
        _value = _baseValue;
        _isDirty = true;
    }

    public float Value
    {
        get
        {
            if (_isDirty)
            {
                CalculateValue();
                _isDirty = false;
            }
            return _value;
        }
    }

    public float MaxValue => _maxValue;
    public float MinValue => _minValue;

    public float Ratio => (_value / _maxValue);

    public void AddModifier(FloatModifier modifier)
    {
        _isDirty = true;
        _modifiers.Add(modifier);
        _modifiers.Sort(CompareModifierType);
    }

    public void RemoveModifier(FloatModifier modifier)
    {
        if (_modifiers.Contains(modifier))
        {
            _isDirty = true;
            _modifiers.Remove(modifier);
            _modifiers.Sort(CompareModifierType);
        }

    }

    public void RemoveAllModifiers()
    {
        _isDirty = true;
        _modifiers.Clear();
    }

    [ContextMenu("Calculate Value")]
    internal void CalculateValue()
    {
        _value = Mathf.Clamp(_baseValue, _minValue, _maxValue);

        float sumPercentAdditive = 0f;
        float finalValue = _value;

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
                    finalValue *= _modifiers[i].Value;
                    break;
            }
        }

        _value = Mathf.Clamp(finalValue, _minValue, _maxValue);
    }

    private int CompareModifierType(FloatModifier x, FloatModifier y)
    {
        if (x.ModifierType > y.ModifierType) return 1;
        if (x.ModifierType < y.ModifierType) return -1;
        return 0;
    }

}
