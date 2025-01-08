using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
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

    [SerializeField] private SerializedDictionary<StateSO, StateContext> _stateContexts = new();
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
        _currentContext?.OnExit();
    }

    private void Update()
    {
        if (_isPaused) return;
        _currentContext?.OnUpdate();
    }

    private void FixedUpdate()
    {
        if (_isPaused) return;
        _currentContext?.OnFixedUpdate();
    }

    internal void Transition()
    {
        StateSO bestState = null;
        float highestUtility = 0f;


        if (ActiveContext == null) return;


        foreach (var context in _stateContexts)
        {
            float utility = context.Value.EvaluateUtility();

            if (utility > highestUtility)
            {
                highestUtility = utility;
                bestState = context.Key;

            }
        }

        if (bestState != null && bestState != _currentState)
        {
            ChangeState(bestState);
        }
    }

    internal void ChangeState(StateSO state)
    {
        if (_currentContext != null)
        {
            _currentContext.OnExit();
        }

        _currentState = state;
        Debug.Log(_currentState);

        if (_currentState != null)
        {
            if (!_stateContexts.TryGetValue(_currentState, out var context))
            {
                context = GetOrCreateContext(_currentState);
                _stateContexts.Add(_currentState, context);
            }
            _currentContext = context;
            _currentContext.OnEnter();
            Debug.Log(_currentContext);
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
        foreach (var context in _stateContexts)
        {

            //context.Value.Dequeue().Reset();
        }
    }

    private void InitializeContext()
    {
        foreach (var state in _states.GetStates())
        {
            _stateContexts[state] = GetOrCreateContext(state);
            _stateContexts[state].Initialize(Agent, this, state);
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
