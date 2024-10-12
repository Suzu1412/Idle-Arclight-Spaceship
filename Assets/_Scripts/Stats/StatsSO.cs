using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "Scriptable Objects/Stats/Character Stats")]
public class StatsSO : SerializableScriptableObject
{
    [SerializedDictionary("Stat type", "Stat")]
    [SerializeField] private SerializedDictionary<StatType, Stat> _stats = new();

    private void OnEnable()
    {
        Recalculate();
    }

    internal void AddStat(Stat newStat)
    {
        _stats.TryAdd(newStat.StatInfo.StatType, newStat);
    }

    internal float GetStatValue(StatType statType)
    {
        if (!_stats.TryGetValue(statType, out var stat))
        {
            Debug.LogError($"{statType} not found on {this.name}");
            return 0;

        }

        return stat.Value;
    }

    internal float GetStatMinValue(StatType statType)
    {
        if (!_stats.TryGetValue(statType, out var stat))
        {
            Debug.LogError($"{statType} not found on {this.name}");
            return 0;
        }

        return stat.StatInfo.MinValue;
    }

    internal float GetStatMaxValue(StatType statType)
    {
        if (!_stats.TryGetValue(statType, out var stat))
        {
            Debug.LogError($"{statType} not found on {this.name}");
            return 0;
        }

        return stat.StatInfo.MaxValue;
    }

    internal Stat GetStat(StatType statType)
    {
        if (!_stats.TryGetValue(statType, out var stat))
        {
            Debug.LogError($"{statType} not found on {this.name}");
            return null;
        }

        return stat;
    }

    [ContextMenu("Recalculate")]
    private void Recalculate()
    {
        foreach (var stat in _stats)
        {
            stat.Value.CalculateValue();
        }
    }

    [ContextMenu("Randomize")]
    private void Random_stats()
    {
        foreach (var stat in _stats)
        {
            stat.Value.BaseValue = Mathf.Round(UnityEngine.Random.Range(stat.Value.StatInfo.MinValue, stat.Value.StatInfo.MaxValue));
            stat.Value.CalculateValue();
            Debug.Log($"{stat.Value.StatInfo.StatType}: {stat.Value.Value}");
        }
    }

    internal void RemoveModifiers()
    {
        foreach (var stat in _stats)
        {
            stat.Value.RemoveAllModifiers();
        }
    }
}
