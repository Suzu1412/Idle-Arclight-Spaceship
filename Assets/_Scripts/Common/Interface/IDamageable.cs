using UnityEngine;

public interface IDamageable
{
    void Damage(int amount, bool ignoreInvulnerability = false);
}