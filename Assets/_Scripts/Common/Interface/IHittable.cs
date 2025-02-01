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
    void SetInvulnerability(bool isInvulnerable, float duration, GameObject source);


    event Action OnHit;
    event Action OnHitStun;
}
