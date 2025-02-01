using UnityEngine;
using UnityEngine.Events;

public class EnemyInput : MonoBehaviour, IAgentInput
{
    public Vector2 Direction { get; private set; }

    public event UnityAction<Vector2> OnMovement;
    public event UnityAction<bool> Attack;

    public void CallOnAttack(bool wasPressed)
    {
        Attack?.Invoke(wasPressed);
    }

    public void CallOnMovementInput(Vector2 direction)
    {
        Direction = direction.normalized;
        OnMovement?.Invoke(Direction);
    }
}
