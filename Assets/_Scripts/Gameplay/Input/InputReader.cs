using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, GameInput.IPlayerActions, GameInput.IMenuToggleActions, IAgentInput
{
    private GameInput _inputActions;
    private Vector2 _direction;
    private bool _isShopActive;
    private bool _isMenuActive;
    [Header("Events")]
    [SerializeField] private Vector2GameEventListener OnStickChangeDirectionListener = default;
    [SerializeField] private BoolGameEvent OnToggleShopEvent;
    [SerializeField] private BoolGameEvent OnToggleMenuEvent;
    [SerializeField] private BoolGameEvent OnGameplayPauseEvent;
    [SerializeField] private BoolVariableSO _isPaused;

    [Header("Sound Data")]
    [SerializeField] private SoundDataSO _confirmSound;
    [SerializeField] private SoundDataSO _cancelSound;

    public Vector2 Direction => _direction;
    public event UnityAction<Vector2> OnMovement;
    public event UnityAction<bool> Attack;

    private void Awake()
    {
        _inputActions = new GameInput();
    }

    private void Start()
    {
        EnablePlayerActions();
        EnableMenuToggleActions();
    }

    private void OnEnable()
    {
        OnStickChangeDirectionListener.Register(CallOnMovementInput);
        _isShopActive = false;
        _isMenuActive = false;
        OnGameplayPauseEvent.RaiseEvent(false, this);
        DisableUIActions();
    }

    private void OnDisable()
    {
        OnStickChangeDirectionListener.DeRegister(CallOnMovementInput);
        _direction = Vector2.zero;
    }

    private void OnDestroy()
    {
        DisablePlayerActions();
        DisableMenuToggleActions();
        DisableUIActions();
    }


    public void EnablePlayerActions()
    {
        _inputActions.Player.SetCallbacks(this);
        _inputActions.Player.Enable();
    }

    public void DisablePlayerActions()
    {
        _inputActions.Player.Disable();
    }

    public void EnableUIActions()
    {
        _inputActions.UI.Enable();
    }

    public void DisableUIActions()
    {
        _inputActions.UI.Disable();
    }

    public void EnableMenuToggleActions()
    {
        _inputActions.MenuToggle.SetCallbacks(this);
        _inputActions.MenuToggle.Enable();
    }

    public void DisableMenuToggleActions()
    {
        _inputActions.MenuToggle.Disable();
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

    public void OnToggleShop(InputAction.CallbackContext context)
    {
        if (_isMenuActive) return;

        if (context.started)
        {
            _isShopActive = !_isShopActive;
            OnToggleShopEvent.RaiseEvent(_isShopActive, this);

            if (!_isShopActive)
            {
                EnablePlayerActions();
                DisableUIActions();
                _cancelSound.PlayEvent();
                OnGameplayPauseEvent.RaiseEvent(false, this);
                _isPaused.Value = false;


            }
            else
            {
                DisablePlayerActions();
                DisableUIActions();
                _confirmSound.PlayEvent();
                OnGameplayPauseEvent.RaiseEvent(true, this);
                _isPaused.Value = true;


            }
        }
    }

    public void OnTogglePauseMenu(InputAction.CallbackContext context)
    {
        if (_isShopActive) return;


        if (context.started)
        {
            _isMenuActive = !_isMenuActive;
            OnToggleMenuEvent.RaiseEvent(_isMenuActive, this);

            if (!_isMenuActive)
            {
                EnablePlayerActions();
                DisableUIActions();
                _cancelSound.PlayEvent();
                OnGameplayPauseEvent.RaiseEvent(false, this);
                _isPaused.Value = false;


            }
            else
            {
                DisablePlayerActions();
                _confirmSound.PlayEvent();
                OnGameplayPauseEvent.RaiseEvent(true, this);
                _isPaused.Value = true;

            }
        }
    }

    public void OnToggleStats(InputAction.CallbackContext context)
    {
        // throw new System.NotImplementedException();
    }

    public void OnToggleQuest(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnCloseAllMenus(InputAction.CallbackContext context)
    {
        if (_isShopActive || _isMenuActive)
        {
            _isShopActive = false;
            OnToggleShopEvent.RaiseEvent(_isShopActive, this);
            _isMenuActive = false;
            OnToggleMenuEvent.RaiseEvent(_isMenuActive, this);

            EnablePlayerActions();
            DisableUIActions();
            _cancelSound.PlayEvent();
            OnGameplayPauseEvent.RaiseEvent(false, this);
            _isPaused.Value = false;
        }

    }
}
