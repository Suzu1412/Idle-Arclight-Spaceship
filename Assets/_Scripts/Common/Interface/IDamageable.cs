using UnityEngine;

public interface IDamageable
{
    void Damage(int amount, bool ignoreInvulnerability = false);
    void Death(bool activateDeathEvents = true);
}