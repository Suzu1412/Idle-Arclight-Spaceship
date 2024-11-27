using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IStatsSystem
{
    event Action OnMaxHealthChange;
    void SetStatsData(IStatsData statsData);

    float GetStatValue(StatType statType);

    float GetStatMaxValue(StatType statType);

    float GetStatMinValue(StatType statType);

    void AddModifier(IStatModifier modifier);

    void RemoveModifier(IStatModifier modifier);

    void AddTemporaryModifier(IStatModifier modifier, float duration);
}
