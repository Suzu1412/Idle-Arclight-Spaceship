using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHealthSystem : IDamageable, IHealable, IHittable, IKillable, IRemovable
{
    event Action<IntVariableSO> OnMaxHealthValueChanged;
    event Action OnHealthValueChanged;
    event Action<int> OnHealed;
    event Action<int> OnDamaged;
    event Action OnInvulnerabilityPeriod;
    bool IsHurt { get; }
    bool IsDeath { get; }

    void Initialize(int currentHealth);
    void SetInvulnerability(bool isInvulnerable, float duration);
    int GetMaxHealth();
    int GetCurrentHealth();
    float GetHealthPercent();
}
