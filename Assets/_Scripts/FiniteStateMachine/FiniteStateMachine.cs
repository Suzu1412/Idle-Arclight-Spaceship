using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private IAgent _agent;
    private BaseState _currentState;
    private Coroutine _transitionCoroutine;
    private float _handleTransitionTime = 0.1f;
    [SerializeField] [ReadOnly] BaseStateSO _debugCurrentState;
    [SerializeField] private List<BaseStateSO> _stateList;
    private Dictionary<BaseStateSO, BaseState> _stateDictionary = new();
    internal IAgent Agent => _agent ??= GetComponent<IAgent>();

    private void Awake()
    {
        for(int i=0; i < _stateList.Count; i++)
        {
            InitializeState(_stateList[i]);
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
        var state = stateSO.State;
        state.Initialize(Agent, this);
        if (!_stateDictionary.TryAdd(stateSO, state))
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

        if (!_stateDictionary.TryGetValue(newStateSO, out var newState))
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
