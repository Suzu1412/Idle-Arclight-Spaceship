using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class InputReader : MonoBehaviour, GameInput.IClickerActions, IAgentInput
{
    private GameInput _inputActions;
    private Vector2 _direction;
    private Camera _mainCamera;
    private Vector3 _offset;
    private Agent _agent;

    public event UnityAction<bool> OnTouchPressed;
    public event UnityAction<Vector2> OnMovement;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _agent = GetComponentInChildren<Agent>();
        _inputActions = new GameInput();
    }

    private void OnEnable()
    {
        EnableClickerActions();
    }

    private void Update()
    {
        if (Touch.activeTouches.Count == 0 ||
            Touch.activeTouches[0].finger.index != 0)
        {
            return;
        }

        Touch myTouch = Touch.activeTouches[0];
        Vector3 touchPos = myTouch.screenPosition;
        touchPos = _mainCamera.ScreenToWorldPoint(touchPos);
        touchPos.z = _agent.transform.position.z;

        if (Touch.activeTouches[0].phase == TouchPhase.Began)
        {
            _offset = touchPos - transform.position;
        }

        if (Touch.activeTouches[0].phase == TouchPhase.Moved)
        {
            //_direction.Set(touchPos.x - _offset.x, touchPos.y - _offset.y);
            
            //CallOnMovementInput(_offset.normalized);
        }
        if (Touch.activeTouches[0].phase == TouchPhase.Moved)
        {
            transform.position = new Vector3(touchPos.x - _offset.x, touchPos.y - _offset.y, 0f);
        }
        if (Touch.activeTouches[0].phase == TouchPhase.Stationary)
        {
            transform.position = new Vector3(touchPos.x - _offset.x, touchPos.y - _offset.y, 0f);
        }
    }

    public void EnableClickerActions()
    {
        _inputActions.Clicker.SetCallbacks(this);
        _inputActions.Enable();
        EnhancedTouchSupport.Enable();
    }

    public void DisableClickerActions()
    {
        _inputActions.Disable();
        EnhancedTouchSupport.Disable();
    }


    public void OnTouchPosition(InputAction.CallbackContext context)
    {
        // CallOnMovementInput(_mainCamera.ScreenToWorldPoint(context.ReadValue<Vector2>()));

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

    public void OnMove(InputAction.CallbackContext context)
    {
        CallOnMovementInput(context.ReadValue<Vector2>());
    }
}
