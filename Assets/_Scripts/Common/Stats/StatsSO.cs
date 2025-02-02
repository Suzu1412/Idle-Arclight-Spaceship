using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Stats/Character Stats")]
public class StatsSO : SerializableScriptableObject, IStatsData
{
    [SerializeField] private List<StatConfig> _initialStats = new();
    public List<StatConfig> InitialStats => _initialStats;

    private void OnEnable()
    {
        Recalculate();
    }

    private void OnValidate()
    {
        Recalculate();
    }

    [ContextMenu("Recalculate")]
    private void Recalculate()
    {
        foreach (var stat in _initialStats)
        {
            stat.Recalculate();
        }
    }

#if UNITY_EDITOR

    [ContextMenu("Load All")]
    private void LoadAll()
    {
        var stats = ScriptableObjectUtilities.FindAllScriptableObjectsOfType<StatComponentSO>("t:StatComponentSO", "Assets/_Data/Stats/Stat Types");

        _initialStats = new();

        foreach (var stat in stats)
        {
            StatConfig statConfig = new(stat);
            _initialStats.Add(statConfig);

        }
    }
#endif
}

[System.Serializable]
public class StatConfig
{
    [SerializeField] private StatComponentSO _statComponent;
    [SerializeField] private float _baseValue;

    public StatComponentSO StatComponent => _statComponent;

    public float BaseValue => _baseValue;

    public StatConfig(StatComponentSO statComponent)
    {
        _statComponent = statComponent;
        _baseValue = _statComponent.MinValue;
    }

    public Stat CreateStat()
    {
        return _statComponent.CreateStat(this);
    }

    public void Recalculate()
    {
        if (StatComponent == null) return;

        if (_baseValue < _statComponent.MinValue)
        {
            _baseValue = _statComponent.MinValue;
        }

        if (_baseValue > _statComponent.MaxValue)
        {
            _baseValue = _statComponent.MaxValue;
        }
    }
}