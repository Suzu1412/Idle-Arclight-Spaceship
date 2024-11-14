using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Double Variable", menuName = "Scriptable Objects/DoubleVariableSO")]
public class DoubleVariableSO : ScriptableObject
{
    [SerializeField] private double _value;
    [SerializeField] private double _minValue;
    [SerializeField] private double _maxValue;

    private bool _isDirty = false;

    private void OnEnable()
    {
        _isDirty = true;
        CalculateValue();
    }

    private void OnValidate()
    {
        _isDirty = true;
        CalculateValue();
    }

    public void Initialize(double value, double minValue = double.MinValue, double maxValue = double.MaxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        _value = value;
        CalculateValue();
        _isDirty = true;
    }

    public double Value
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

    public double MaxValue
    {
        get => _maxValue;
        set
        {
            _isDirty = true;
            MaxValue = value;
        }
    }
    public double MinValue => _minValue;

    public float Ratio => (float)(_value / _maxValue);

    [ContextMenu("Calculate Value")]
    private void CalculateValue()
    {
        _value = Math.Clamp(_value, _minValue, _maxValue);
    }
}
