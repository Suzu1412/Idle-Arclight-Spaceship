using UnityEngine;

public interface IDamageable
{
    void Damage(int amount);
    void Death(bool invokeEvents = true);
}