using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntVariableSO", menuName = "Scriptable Objects/IntVariableSO")]
public class IntVariableSO : ScriptableObject
{
    [SerializeField] private int _value;
    [SerializeField] private int _minValue;
    [SerializeField] private int _maxValue;

    private bool _isDirty = false;

    private void OnEnable()
    {
        _isDirty = true;
        _value = Mathf.Clamp(_value, _minValue, _maxValue);
    }

    private void OnValidate()
    {
        _isDirty = true;
        _value = Mathf.Clamp(_value, _minValue, _maxValue);
    }

    public void Initialize(int value, int minValue, int maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        _value = Mathf.Clamp(_value, _minValue, _maxValue);
        _isDirty = true;
    }

    public int Value
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
        set
        {
            _isDirty = true;
            _value = value;
        }
    }

    public int MaxValue
    {
        get => _maxValue;
        set
        {
            _isDirty = true;
            MaxValue = value;
        }
    }
    public int MinValue => _minValue;

    public float Ratio => (float)_value / _maxValue;

    [ContextMenu("Calculate Value")]
    internal void CalculateValue()
    {
        _value = Mathf.Clamp(_value, _minValue, _maxValue);
    }
}
