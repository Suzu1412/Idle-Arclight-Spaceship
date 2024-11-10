using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, GameInput.IClickerActions, IAgentInput
{
    private GameInput _inputActions;
    private Vector2 _direction;
    [SerializeField] private Vector2GameEventListener OnStickChangeDirectionListener = default;

    public Vector2 Direction => _direction;
    public event UnityAction<Vector2> OnMovement;
    public event UnityAction<Vector2> OnSetDestination;
    public event UnityAction<bool> Attack;

    private void Awake()
    {
        _inputActions = new GameInput();
    }

    private void OnEnable()
    {
        EnableClickerActions();
        OnStickChangeDirectionListener.Register(CallOnMovementInput);
    }

    private void OnDisable()
    {
        OnStickChangeDirectionListener.DeRegister(CallOnMovementInput);
        _direction = Vector2.zero;
    }


    public void EnableClickerActions()
    {
        _inputActions.Clicker.SetCallbacks(this);
        _inputActions.Enable();
    }

    public void DisableClickerActions()
    {
        _inputActions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        CallOnMovementInput(context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) CallOnAttack(true);
        if (context.canceled) CallOnAttack(false);
    }

    public void CallOnMovementInput(Vector2 direction)
    {
        _direction = direction;
        OnMovement?.Invoke(_direction);
    }

    public void CallOnAttack(bool wasPressed)
    {
        Attack?.Invoke(wasPressed);
    }
}
