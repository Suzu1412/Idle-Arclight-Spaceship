using UnityEngine;
using System;

public class StatsSystem : MonoBehaviour, IStatsSystem
{
    [SerializeField] private StatsSO _stats;

    public event Action OnMaxHealthChange;

    private void OnDisable()
    {
        _stats.RemoveModifiers();
    }

    internal void AddStat(Stat stat)
    {
        _stats.AddStat(stat);
    }

    public float GetStatValue(StatType statType)
    {
        return _stats.GetStatValue(statType);
    }

    public float GetStatMaxValue(StatType statType)
    {
        return _stats.GetStatMaxValue(statType);
    }

    public float GetStatMinValue(StatType statType)
    {
        return _stats.GetStatMinValue(statType);
    }

    internal void RemoveModifier(StatModifier modifier)
    {

    }

    public void AddModifier(IStatModifier modifier)
    {
        if (_stats.GetStat(modifier.StatType) == null)
        {
            Debug.LogError($"Could not find Stat of Type: {modifier.StatType} in {transform.root.name}");
            return;
        }

        _stats.GetStat(modifier.StatType).AddModifier(modifier as StatModifier);
        if (modifier.StatType == StatType.MaxHealth) OnMaxHealthChange?.Invoke();
    }

    public void RemoveModifier(IStatModifier modifier)
    {
        if (_stats.GetStat(modifier.StatType) == null)
        {
            Debug.LogError($"Could not find Stat of Type: {modifier.StatType}", this);
            return;
        }

        _stats.GetStat(modifier.StatType).RemoveModifier(modifier as StatModifier);
        if (modifier.StatType == StatType.MaxHealth) OnMaxHealthChange?.Invoke();
    }

    public async void AddTemporaryModifier(IStatModifier modifier, float duration)
    {
        AddModifier(modifier);
        await Awaitable.WaitForSecondsAsync(duration);
        RemoveModifier(modifier);
    }

    public void SetStatsData(IStatsData statsData)
    {
        _stats = statsData as StatsSO;
    }
}
