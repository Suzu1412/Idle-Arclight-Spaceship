using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour, IPausable
{
    [SerializeField] private PausableRunTimeSetSO _pausable = default;

    private bool _isPaused;
    [SerializeField][ReadOnly] private int _currentPhase = 0;
    private IAgent _agent;
    private Coroutine _transitionCoroutine;
    private float _handleTransitionTime = 0.1f;
    [SerializeField] private StateListSO _states; // All Possible States
    [SerializeField][ReadOnly] private StateSO _currentState;
    [SerializeField] private StateContext _currentContext;

    private Dictionary<StateSO, StateContext> _stateContexts = new();
    private Dictionary<StateSO, Queue<StateContext>> _contextPool = new();
    internal IAgent Agent => _agent ??= GetComponent<IAgent>();

    internal StateContext ActiveContext => _currentContext;

    private void Awake()
    {
        if (_states == null)
        {
            Debug.LogError(gameObject.transform.parent.name + " has no states assigned, please fix");
        }
        InitializeContext();
    }

    private void OnEnable()
    {
        _pausable.Add(this);
        _currentPhase = 0;
        ResetContext();
        ChangePhase(_currentPhase);
        ChangeState(_states.DefaultState);
        _transitionCoroutine = StartCoroutine(TransitionCoroutine());
    }

    private void OnDisable()
    {
        _pausable.Remove(this);
        StopAllCoroutines();
        _currentState?.ExitState(this);
    }

    private void Update()
    {
        if (_isPaused) return;
        _currentState?.UpdateState(this);
    }

    private void FixedUpdate()
    {
        if (_isPaused) return;
        _currentState?.FixedUpdateState(this);
    }

    internal void Transition()
    {
        StateSO bestState = null;
        float highestUtility = 0f;


        if (ActiveContext == null) return;


        foreach (var state in _states.GetPhaseStates())
        {
            float utility = state.EvaluateUtility(this);

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
        if (_currentState != null)
        {
            _currentState.ExitState(this);
            ReturnContextToPool(state, _currentContext);
        }

        _currentState = state;

        if (_currentState != null)
        {
            if (!_stateContexts.TryGetValue(_currentState, out var context))
            {
                context = GetOrCreateContext(_currentState);
                _stateContexts.Add(_currentState, context);
            }
            _currentContext = context; //
            _currentContext.Initialize(Agent);
            _currentState.EnterState(this);
        }
    }

    internal void ChangePhase(int phase)
    {
        _currentPhase = phase;
        _states.UpdatePhaseStates(_currentPhase);
    }


    private IEnumerator TransitionCoroutine()
    {
        while (true)
        {
            Transition();
            yield return Helpers.GetWaitForSeconds(_handleTransitionTime);
        }
    }

    internal StateContext GetContext(StateSO state)
    {
        if (_stateContexts.TryGetValue(state, out var context))
        {
            return context;
        }
        Debug.LogError("Could not find appropiate context for: " + state);
        return null;
    }

    private StateContext GetOrCreateContext(StateSO state)
    {
        if (!_contextPool.TryGetValue(state, out var pool))
        {
            pool = new Queue<StateContext>();
            _contextPool[state] = pool;
        }
        return pool.Count > 0 ? pool.Dequeue() : state.CreateContext();
    }

    private void ReturnContextToPool(StateSO state, StateContext context)
    {
        if (!_contextPool.ContainsKey(state))
        {
            _contextPool[state] = new Queue<StateContext>();
        }

        _contextPool[state].Enqueue(context);
    }

    private void ResetContext()
    {
        foreach(var context in _stateContexts)
        {

            //context.Value.Dequeue().Reset();
        }
    }

    private void InitializeContext()
    {
        foreach(var state in _states.GetStates)
        {
            _stateContexts[state] = GetOrCreateContext(state);
            ReturnContextToPool(state, _stateContexts[state]);
        }
    }

    public void Pause(bool isPaused)
    {
        _isPaused = isPaused;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
