using UnityEngine;
using UnityEngine.Events;

public interface IAgentInput
{
    Vector2 Direction { get; }
    event UnityAction<Vector2> OnMovement;
    event UnityAction<bool> Attack;

    void CallOnMovementInput(Vector2 direction);
    void CallOnAttack(bool wasPressed);
}
