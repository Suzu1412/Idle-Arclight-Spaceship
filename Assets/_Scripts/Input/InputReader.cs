using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, GameInput.IClickerActions, IAgentInput
{
    private GameInput _inputActions;
    private Vector2 _direction;
    private Vector2 _position;
    private Camera _mainCamera;


    public event UnityAction<bool> OnTouchPressed;
    public event UnityAction<Vector2> OnMovement;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _inputActions = new GameInput();
    }

    private void OnEnable()
    {
        EnableClickerActions();
    }

    public void EnableClickerActions()
    {
        _inputActions.Clicker.SetCallbacks(this);
        _inputActions.Enable();
    }


    public void OnTouchPosition(InputAction.CallbackContext context)
    {
        CallOnMovementInput(_mainCamera.ScreenToWorldPoint(context.ReadValue<Vector2>()));

    }

    public void OnTouchPress(InputAction.CallbackContext context)
    {
        if (context.started) CallOnTouchPressed(true);
        if (context.canceled) CallOnTouchPressed(false);
    }

    public void CallOnMovementInput(Vector2 direction)
    {
        _direction = direction;
        OnMovement?.Invoke(_direction);
    }

    public void CallOnTouchPressed(bool touchPressed)
    {
        OnTouchPressed?.Invoke(touchPressed);
    }
}
