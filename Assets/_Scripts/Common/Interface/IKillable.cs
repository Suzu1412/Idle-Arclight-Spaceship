using System;
using UnityEngine;

public interface IKillable 
{
    event Action OnDeath;

    void Death(GameObject source, DeathCauseType cause);
}
