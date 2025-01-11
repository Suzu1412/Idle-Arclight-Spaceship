using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "Scriptable Objects/Stats/Character Stats")]
public class StatsSO : SerializableScriptableObject, IStatsData
{
    [SerializeField] private List<StatConfig> _initialStats = new();

    [SerializeField] private SerializedDictionary<StatComponentSO, Stat> _runtimeStats = new();

    private void OnEnable()
    {
        Recalculate();
    }

    [ContextMenu("Create Stats")]
    public void CreateStats()
    {
        // Initialize runtime stats
        foreach (var component in _initialStats)
        {
            var type = component.StatComponent;
            if (!_runtimeStats.ContainsKey(type))
            {
                _runtimeStats[type] = component.CreateStat();
            }
        }
    }

    public float GetStatValue<T>() where T : StatComponentSO
    {
        return GetStat<T>().Value;
    }

    public float GetStatMinValue<T>() where T : StatComponentSO
    {
        return GetStat<T>().StatComponent.MaxValue;
    }

    public float GetStatMaxValue<T>() where T : StatComponentSO
    {
        return GetStat<T>().StatComponent.MaxValue;
    }


    public Stat GetStat<TStat>() where TStat : StatComponentSO
    {
        var statComponent = _initialStats.Find(s => s.StatComponent is TStat).StatComponent;

        if (statComponent != null && _runtimeStats.TryGetValue(statComponent, out var stat))
        {
            return stat;
        }

        Debug.LogWarning($"Stat of type {typeof(TStat).Name} not found!");
        return null;
    }

    public Stat GetStat(StatComponentSO statComponent)
    {
        if (_runtimeStats.TryGetValue(statComponent, out var stat))
        {
            return stat;
        }

        Debug.LogWarning($"Stat of type {statComponent.name} not found!");
        return null;
    }

    [ContextMenu("Recalculate")]
    private void Recalculate()
    {
        foreach (var stat in _runtimeStats)
        {
            stat.Value.CalculateValue();
        }
    }

    [ContextMenu("Randomize")]
    private void Random_stats()
    {
        foreach (var stat in _runtimeStats)
        {
            stat.Value.BaseValue = Mathf.Round(UnityEngine.Random.Range(stat.Value.StatComponent.MinValue, stat.Value.StatComponent.MaxValue));
            stat.Value.CalculateValue();
            Debug.Log($"{stat.Value.StatComponent.GetType()}: {stat.Value.Value}");
        }
    }

    [ContextMenu("Load All")]
    private void LoadAll()
    {
        var stats = ScriptableObjectUtilities.FindAllScriptableObjectsOfType<StatComponentSO>("t:StatComponentSO", "Assets/_Data/Stats/Stat Types");

        _initialStats = new();

        foreach (var stat in stats)
        {
            StatConfig statConfig = new StatConfig(stat);
            _initialStats.Add(statConfig);

        }
    }

    internal void RemoveModifiers()
    {
        foreach (var stat in _runtimeStats)
        {
            stat.Value.RemoveAllModifiers();
        }
    }
}

[System.Serializable]
public class StatConfig
{
    [SerializeField] private StatComponentSO _statComponent;
    [SerializeField] private float _baseValue;

    public StatComponentSO StatComponent => _statComponent;

    public StatConfig(StatComponentSO statComponent )
    {
        _statComponent = statComponent;
        _baseValue = _statComponent.MinValue;
    }

    public Stat CreateStat()
    {
        return _statComponent.CreateStat(_baseValue);
    }
}