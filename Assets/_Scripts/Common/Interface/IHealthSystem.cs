using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHealthSystem : IDamageable, IHealable, IHittable
{
    int GetMaxHealth();

    int GetCurrentHealth();

    bool IsHurt { get; }
    bool IsDeath { get; }

    event Action<float, float> OnMaxHealthValueChanged;
    event Action<float, float> OnHealthValueChanged;
    event Action<int> OnHealed;
    event Action<int> OnDamaged;
    event Action OnDeath;

    void SetInvulnerability(bool isInvulnerable);

}
