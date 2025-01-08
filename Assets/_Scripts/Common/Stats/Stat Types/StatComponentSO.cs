using UnityEngine;

public abstract class StatComponentSO : ScriptableObject
{
    [SerializeField] protected float _minValue = 0f;

    [SerializeField] protected float _maxValue = 9999f;

    public float MinValue => _minValue;

    public float MaxValue => _maxValue;


    // Creates a new Stat instance for a character
    internal Stat CreateStat(float baseValue)
    {
        return new Stat(this, baseValue);
    }
}

