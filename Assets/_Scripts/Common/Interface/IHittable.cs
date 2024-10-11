using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHittable
{
    float HurtDuration { get; }
    float InvulnerabilityDuration { get; }
    int KnockbackDirection { get; }

    void GetHit(GameObject damageDealer);

    event Action OnHitStun;
}
