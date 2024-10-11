using UnityEngine;
using System;

public class StatsSystem : MonoBehaviour, IStatsSystem
{
    [SerializeField] private StatsSO _stats;
    internal StatsSO Stats => _stats;

    public event Action OnMaxHealthChange;
    public event Action OnMaxManaChange;

    private void OnValidate()
    {
        if (_stats == null) Debug.LogWarning("Please Assign Stats SO", this.gameObject);
    }

    private void Awake()
    {
        _stats.Initialize();
    }

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

    internal void AddModifier(StatModifier modifier)
    {
        if (_stats.GetStat(modifier.StatType) == null)
        {
            Debug.LogError($"Could not find Stat of Type: {modifier.StatType} in {transform.root.name}");
            return;
        }

        _stats.GetStat(modifier.StatType).AddModifier(modifier);
        if (modifier.StatType == StatType.MaxHealth) OnMaxHealthChange?.Invoke();
    }

    internal void RemoveModifier(StatModifier modifier)
    {
        if (_stats.GetStat(modifier.StatType) == null)
        {
            Debug.LogError($"Could not find Stat of Type: {modifier.StatType}", this);
            return;
        }

        _stats.GetStat(modifier.StatType).RemoveModifier(modifier);
        if (modifier.StatType == StatType.MaxHealth) OnMaxHealthChange?.Invoke();
    }

    public void AddModifier(IStatModifier modifier)
    {
        AddModifier(modifier as StatModifier);
    }

    public void RemoveModifier(IStatModifier modifier)
    {
        RemoveModifier(modifier as StatModifier);
    }

    public async void AddTemporaryModifier(IStatModifier modifier, float duration)
    {
        AddModifier(modifier as StatModifier);
        await Awaitable.WaitForSecondsAsync(duration);
        RemoveModifier(modifier as StatModifier);
    }
}
