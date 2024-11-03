using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHittable
{
    float HurtDuration { get; }
    float InvulnerabilityDuration { get; }
    int KnockbackDirection { get; }
    bool IsInvulnerable { get; }

    void GetHit(GameObject damageDealer);

    event Action OnHit;
    event Action OnHitStun;
}
