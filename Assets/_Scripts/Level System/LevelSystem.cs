using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSystem : MonoBehaviour, ILevelSystem
{
    private IAgent _agent;
    private int _previousLevel = 0;
    [SerializeField] private LevelSO _level;
    [SerializeField] private LevelStatsSO _levelStatsModifiers;
    [SerializeField] [ReadOnly] private float _currentExp;
    [SerializeField] [ReadOnly] private float _requiredExp;
    [SerializeField] [ReadOnly] private float _previousRequiredExp;
    [SerializeField] [ReadOnly] private float _totalRequiredExp;
    [SerializeField] private float _totalExp;
    [SerializeField] private FloatGameEventListener OnGainExperience;

    public event Action<float, float> OnRequiredExpChanged;
    public event Action<int> OnLevelGained;
    public event Action<float> OnExpGained;
    public IAgent Agent => _agent ??= _agent = GetComponent<IAgent>();

    [Header("Exp Formula")]
    [SerializeField] [Range(1f, 300f)] private float additionMultiplier = 300f;
    [SerializeField] [Range(2f, 4f)] private float powerMultiplier = 2f;
    [SerializeField] [Range(7f, 14f)] private float divisionMultiplier = 7f;

    private void OnEnable()
    {
        _previousLevel = 0;
        OnGainExperience.Register(GainExperienceFlat);
    }

    private void OnDisable()
    {
        OnGainExperience.DeRegister(GainExperienceFlat);
    }

    private void Awake()
    {
        if (_level == null) _level = ScriptableObject.CreateInstance<LevelSO>();
        _level.Initialize(LoadLevel(), 1, 99);
        _requiredExp = CalculateRequiredExp();
    }

    private void Start()
    {
        Initialize();
    }

    internal void Initialize()
    {
        OnRequiredExpChanged?.Invoke(_currentExp, _requiredExp);
    }

    internal int LoadLevel()
    {
        return CalculateLevel();
    }

    private int CalculateLevel()
    {
        int level = 0;

        return level + 1;
    }

    public void GainExperienceFlat(float expGained)
    {
        if (_level.CurrentValue == 99 || expGained <= 0f) return;

        _totalExp += expGained;
        _currentExp += expGained;

        if (_currentExp >= _requiredExp)
        {
            LevelUp();
            OnLevelGained?.Invoke(_level.CurrentValue);
        }

        OnExpGained?.Invoke(_currentExp);
    }

    private void LevelUp()
    {
        while (_currentExp > _requiredExp)
        {
            _level.CurrentValue++;
            _levelStatsModifiers.AddModifiers(_previousLevel, _level.CurrentValue, Agent);
            _previousLevel = _level.CurrentValue;
            _currentExp = Mathf.RoundToInt(_currentExp - _requiredExp);
            _previousRequiredExp = _requiredExp;
            _totalRequiredExp = CalculateRequiredExp();
            _requiredExp = CalculateRequiredExp() - _previousRequiredExp;

            if (_level.CurrentValue == 99)
            {
                _totalExp = _totalRequiredExp;
                _currentExp = _requiredExp;
                break;
            }
        }

        OnRequiredExpChanged?.Invoke(_currentExp, _requiredExp);
    }

    private int CalculateRequiredExp()
    {
        int solveForRequiredExp = 0;
        float step = 0;

        for (int levelCycle = 1; levelCycle <= _level.CurrentValue; levelCycle++)
        {
            step = Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier);
            solveForRequiredExp += Mathf.FloorToInt(levelCycle + additionMultiplier * step);
        }

        return solveForRequiredExp / 4;
    }

    public float GetCurrentExp()
    {
        return _currentExp;
    }
}
