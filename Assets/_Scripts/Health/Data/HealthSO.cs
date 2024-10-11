using UnityEngine;

[CreateAssetMenu(menuName = "Health", fileName = "Health_")]
public class HealthSO : ScriptableObject, IHealth
{
    [SerializeField] private int _maxValue;
    [SerializeField] private int _currentValue;
    private int _minPossibleValue;
    private int _maxPossibleValue;

    public void Initialize(int maxValue, int minPossibleValue, int maxPossibleValue)
    {
        _minPossibleValue = minPossibleValue;
        _maxPossibleValue = maxPossibleValue;
        MaxValue = maxValue;
        CurrentValue = maxValue;
    }

    public int MaxValue
    {
        get => _maxValue;
        internal set
        {
            _maxValue = Mathf.Clamp(value, _minPossibleValue, _maxPossibleValue);

            if (_currentValue > _maxValue)
            {
                CurrentValue = _maxValue;
            }
        }
    }

    public int CurrentValue
    {
        get => _currentValue;
        internal set
        {
            _currentValue = Mathf.Clamp(value, 0, _maxValue);
        }
    }
}
