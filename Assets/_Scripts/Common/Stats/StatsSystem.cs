using UnityEngine;
using System;

public class StatsSystem : MonoBehaviour
{
    [SerializeField] private StatsSO _stats;

    public event Action OnMaxHealthChange;

    private void OnDisable()
    {
        _stats.RemoveModifiers();
    }

    private void Awake()
    {
        _stats.CreateStats();
    }

    public Stat GetStat(StatComponentSO statComponent)
    {
        return _stats.GetStat(statComponent);
    }

    public Stat GetStat<T>() where T : StatComponentSO
    {
        return _stats.GetStat<T>();
    }

    public float GetStatValue<T>() where T : StatComponentSO
    {
        return _stats.GetStatValue<T>();
    }

    public float GetStatMaxValue<T>() where T : StatComponentSO
    {
        return _stats.GetStatMaxValue<T>();
    }

    public float GetStatMinValue<T>() where T : StatComponentSO
    {
        return _stats.GetStatMinValue<T>();
    }

    public void AddModifier(StatModifier modifier)
    {
        var stat = GetStat(modifier.StatComponent);
        stat.AddModifier(modifier);
        if (modifier.StatComponent is HealthStatSO) OnMaxHealthChange?.Invoke();
    }

    public void RemoveModifier(StatModifier modifier)
    {
        var stat = GetStat(modifier.StatComponent);
        stat.RemoveModifier(modifier);
        if (modifier.StatComponent is HealthStatSO) OnMaxHealthChange?.Invoke();

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
    }
}
