using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour, IPausable
{
    [SerializeField] private PausableRunTimeSetSO _pausable = default;
    [SerializeField] private BoolVariableSO _isPaused;
    [SerializeField][Tooltip("Agent - Enemy - Boss RTS goes here")] private GameObjectRuntimeSetSO _activeCharacterRTS;

    private IAgent _agent;
    private Coroutine _transitionCoroutine;
    private float _handleTransitionTime = 0.1f;
    [SerializeField] private StateListSO _states; // All Possible States
    [SerializeField][ReadOnly] private StateSO _currentState;
    [SerializeReference] private StateContext _currentContext;

    internal IAgent Agent => _agent ??= GetComponent<IAgent>();

    public BoolVariableSO IsPaused { get => _isPaused; set => _isPaused = value; }

    private void OnEnable()
    {
        _pausable.Add(this);
        _activeCharacterRTS.Add(gameObject);
        _transitionCoroutine = StartCoroutine(TransitionCoroutine());
        Agent.Input.OnMovement += HandleMovement;
        Agent.Input.Attack += HandleAttack;
    }

    private void OnDisable()
    {
        _pausable.Remove(this);
        _activeCharacterRTS.Remove(gameObject);
        StopAllCoroutines();
        Agent.Input.OnMovement -= HandleMovement;
        Agent.Input.Attack -= HandleAttack;

        if (_currentState == null || _currentContext == null) return;
        _currentState.OnExit(_currentContext);

    }

    private void Update()
    {
        if (_isPaused.Value) return;
        if (_currentState == null || _currentContext == null) return;
        _currentState?.OnUpdate(_currentContext);
    }

    private void FixedUpdate()
    {
        if (_isPaused.Value) return;
        if (_currentState == null || _currentContext == null) return;
        _currentState.OnFixedUpdate(_currentContext);
    }

    public void SetStates(StateListSO states)
    {
        _states = states;
        InitializeContext();
        ChangeState(_states.DefaultState);
    }


    internal void Transition()
    {
        StateSO bestState = null;
        float highestUtility = 0f;

        if (_currentState == null || _currentContext == null) return;

        foreach (var state in _states.GetStates())
        {
            float utility = state.EvaluateUtility(_currentContext);

            if (utility > highestUtility)
            {
                highestUtility = utility;
                bestState = state;
            }
        }

        if (bestState != null && bestState != _currentState)
        {
            ChangeState(bestState);
        }
    }

    internal void ChangeState(StateSO state)
    {
        if (_currentState != null && _currentContext != null)
        {
            _currentState.OnExit(_currentContext);
        }

        _currentState = state;

        if (_currentState != null && _currentContext != null)
        {
            _currentState.OnEnter(_currentContext);
        }
    }

    private IEnumerator TransitionCoroutine()
    {
        while (true)
        {
            Transition();
            yield return Helpers.GetWaitForSeconds(_handleTransitionTime);
        }
    }

    private void InitializeContext()
    {
        _currentContext = _states.DefaultState.CreateContext();
        _currentContext.Initialize(Agent, this);
    }

    private void OnDrawGizmos()
    {
        if (_currentState == null || _currentContext == null) return;
        _currentState.DrawGizmos(_currentContext);
    }

    private void HandleMovement(Vector2 direction)
    {
        if (_currentState == null || _currentContext == null) return;
        _currentState.HandleMovement(_currentContext, direction);
    }

    private void HandleAttack(bool isAttacking)
    {
        if (_currentState == null || _currentContext == null) return;
        _currentState.HandleAttack(_currentContext, isAttacking);
    }

    public void Pause(bool isPaused)
    {
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
