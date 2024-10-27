using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private IAgent _agent;
    private BaseState _currentState;
    private Coroutine _transitionCoroutine;
    private float _handleTransitionTime = 0.1f;
    [SerializeField] [ReadOnly] BaseStateSO _debugCurrentState;
    [SerializeField] private GlobalStateListSO _globalStates;
    [SerializeField] private List<BaseStateSO> _stateList;
    [SerializeField] private readonly Dictionary<BaseStateSO, BaseState> _states = new();
    internal IAgent Agent => _agent ??= GetComponent<IAgent>();
    internal GlobalStateListSO GlobalStates => _globalStates;

    private void Awake()
    {
        foreach (var state in _stateList)
        {
            InitializeState(state);
        }
    }

    private void OnEnable()
    {
        Transition(_stateList[0]);
        _transitionCoroutine = StartCoroutine(TransitionCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _currentState?.OnExit();
    }

    private void Update()
    {
        _currentState?.OnUpdate();
    }

    private void FixedUpdate()
    {
        _currentState?.OnFixedUpdate();
    }

    private BaseState InitializeState(BaseStateSO stateSO)
    {
        var state = stateSO.CreateState();
        state.Initialize(Agent, stateSO, this);
        if (!_states.TryAdd(stateSO, state))
        {
            Debug.LogError($"{this.gameObject} already have {stateSO}. Please Remove");
            return null;
        }

        return state;
    }


    internal void Transition(BaseStateSO newStateSO)
    {
        if (newStateSO == null)
        {
            return;
        }

        _currentState?.OnExit();

        if (!_states.TryGetValue(newStateSO, out var newState))
        {
            newState = InitializeState(newStateSO);
        }

        _currentState = newState;
        _debugCurrentState = newStateSO;
        _currentState?.OnEnter();
    }

    private IEnumerator TransitionCoroutine()
    {
        while (true)
        {
            Transition(_currentState?.HandleTransition());
            yield return new WaitForSeconds(_handleTransitionTime);
        }
    }
}
