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
    [SerializeField] private Vector2GameEventListener OnStickChangeDirectionListener = default;

    public event UnityAction<bool> OnTouchPressed;
    public event UnityAction<Vector2> OnMovement;
    public event UnityAction<Vector2> OnSetDestination;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _agent = GetComponentInChildren<Agent>();
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
    }

    //private void Update()
    //{

    //    if (Touch.activeTouches.Count == 0 ||
    //        Touch.activeTouches[0].finger.index != 0)
    //    {
    //        return;
    //    }

    //    Touch myTouch = Touch.activeTouches[0];
    //    Vector3 touchPos = myTouch.screenPosition;
    //    Vector3 destination = Vector3.zero;
    //    touchPos = _mainCamera.ScreenToWorldPoint(touchPos);
    //    touchPos.z = _agent.transform.position.z;

    //    if (Touch.activeTouches[0].phase == TouchPhase.Began)
    //    {
    //        _offset = touchPos - transform.GetChild(0).position;
    //    }

    //    if (Touch.activeTouches[0].phase == TouchPhase.Moved)
    //    {
    //        //_direction.Set(touchPos.x - _offset.x, touchPos.y - _offset.y);

    //        //CallOnMovementInput(_offset.normalized);
    //    }
    //    if (Touch.activeTouches[0].phase == TouchPhase.Moved)
    //    {
    //        destination = new Vector3(touchPos.x - _offset.x, touchPos.y - _offset.y, 0f);
    //        OnSetDestination(destination);
    //    }
    //    if (Touch.activeTouches[0].phase == TouchPhase.Stationary)
    //    {
    //        destination = new Vector3(touchPos.x - _offset.x, touchPos.y - _offset.y, 0f);
    //        //OnSetDestination(destination);
    //        //new Vector3(touchPos.x - _offset.x, touchPos.y - _offset.y, 0f);
    //    }

    //    if (Touch.activeTouches[0].phase == TouchPhase.Ended)
    //    {
    //        destination = new Vector3(touchPos.x - _offset.x, touchPos.y - _offset.y, 0f);
    //        OnSetDestination(destination);
    //    }
    //}

    public void EnableClickerActions()
    {
        _inputActions.Clicker.SetCallbacks(this);
        _inputActions.Enable();
        EnhancedTouchSupport.Disable();
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
