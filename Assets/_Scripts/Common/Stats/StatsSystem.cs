using UnityEngine;
using System;
using AYellowpaper.SerializedCollections;

public class StatsSystem : MonoBehaviour
{
    [SerializeField] private StatsSO _stats;
    [SerializeField] private SerializedDictionary<StatComponentSO, Stat> _runtimeStats = new();

    public event Action OnMaxHealthChange;

    private void OnDisable()
    {
        RemoveAllModifiers();
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

    public Stat GetStat<T>() where T : StatComponentSO
    {
        var statComponent = _stats.InitialStats.Find(s => s.StatComponent is T).StatComponent;

        if (statComponent != null && _runtimeStats.TryGetValue(statComponent, out var stat))
        {
            return stat;
        }

        Debug.LogWarning($"Stat of type {typeof(T).Name} not found!");
        return null;
    }

    public float GetStatValue<T>() where T : StatComponentSO
    {
        return GetStat<T>().Value;
    }

    public float GetStatMaxValue<T>() where T : StatComponentSO
    {
        return GetStat<T>().StatComponent.MaxValue;
    }

    public float GetStatMinValue<T>() where T : StatComponentSO
    {
        return GetStat<T>().StatComponent.MinValue;
    }

    public void AddModifier(StatModifier modifier)
    {
        GetStat(modifier.StatComponent).AddModifier(modifier);
        if (modifier.StatComponent is HealthStatSO) OnMaxHealthChange?.Invoke();
    }

    public void RemoveModifier(StatModifier modifier)
    {
        GetStat(modifier.StatComponent).RemoveModifier(modifier);
        if (modifier.StatComponent is HealthStatSO) OnMaxHealthChange?.Invoke();

    }

    public void RemoveAllModifiers()
    {
        foreach (var stat in _runtimeStats)
        {
            stat.Value.RemoveAllModifiers();
        }
    }

    public async void AddTemporaryModifier(StatModifier modifier, float duration)
    {
        AddModifier(modifier);
        await Awaitable.WaitForSecondsAsync(duration);
        RemoveModifier(modifier);
    }

    public void SetStatsData(IStatsData statsData)
    {
        _stats = statsData as StatsSO;
        Initialize();
    }

    private void Initialize()
    {
        foreach (var stat in _stats.InitialStats)
        {
            if (!_runtimeStats.ContainsKey(stat.StatComponent))
            {
                _runtimeStats[stat.StatComponent] = stat.CreateStat();
            }
            else
            {
                _runtimeStats[stat.StatComponent].BaseValue = stat.BaseValue;
            }
        }
    }
}
