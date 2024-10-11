using UnityEngine;
using UnityEngine.Events;

public interface IAgentInput
{
    event UnityAction<bool> OnTouchPressed;
    event UnityAction<Vector2> OnMovement;

    void CallOnMovementInput(Vector2 direction);
    void CallOnTouchPressed(bool touchPressed);
}
