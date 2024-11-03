using UnityEngine;

public interface IAttack
{
    LayerMask ProjectileMask { get; }
    void Attack(bool isPressed);
}
