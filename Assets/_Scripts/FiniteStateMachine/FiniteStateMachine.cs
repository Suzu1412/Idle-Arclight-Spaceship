using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour, IPausable
{
    [SerializeField] private PausableRunTimeSetSO _pausable = default;

    private bool _isPaused;
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
        for (int i = 0; i < _stateList.Count; i++)
        {
            InitializeState(_stateList[i]);
        }
    }

    private void OnEnable()
    {
        _pausable.Add(this);
        Transition(_stateList[0]);
        _transitionCoroutine = StartCoroutine(TransitionCoroutine());
    }

    private void OnDisable()
    {
        _pausable.Remove(this);
        StopAllCoroutines();
        _currentState?.OnExit();
    }

    private void Update()
    {
        if (_isPaused) return;
        _currentState?.OnUpdate();
    }

    private void FixedUpdate()
    {
        if (_isPaused) return;
        _currentState?.OnFixedUpdate();
    }

    private BaseState InitializeState(BaseStateSO stateSO)
    {
        var state = stateSO.CreateState();
        state.Initialize(Agent, stateSO, this);
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
            yield return Helpers.GetWaitForSeconds(_handleTransitionTime);
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
