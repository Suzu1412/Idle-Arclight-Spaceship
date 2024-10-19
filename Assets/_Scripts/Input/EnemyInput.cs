using UnityEngine;
using UnityEngine.Events;

public class EnemyInput : MonoBehaviour, IAgentInput
{
    public event UnityAction<bool> OnTouchPressed;
    public event UnityAction<Vector2> OnMovement;
    public event UnityAction<Vector2> OnSetDestination;

    public void CallOnMovementInput(Vector2 direction)
    {
        OnMovement?.Invoke(direction);
    }

    public void CallOnTouchPressed(bool touchPressed)
    {
        OnTouchPressed?.Invoke(touchPressed);
    }
}
