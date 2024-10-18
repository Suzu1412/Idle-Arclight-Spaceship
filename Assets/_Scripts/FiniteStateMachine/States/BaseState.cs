using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseState
{
    protected IAgent _agent;
    protected BaseStateSO _stateOrigin;
    [SerializeField] protected float _stateTime;
    [SerializeField] protected HashSet<BaseTransitionSO> _transitions;
    protected FiniteStateMachine _machine;
    protected int index;

    public void Initialize(IAgent agent, BaseStateSO stateOrigin, FiniteStateMachine machine)
    {
        _agent = agent;
        _stateOrigin = stateOrigin;
        _transitions = new();

        foreach (var transition in stateOrigin.Transitions)
        {
            AddTransition(transition);
        }
        _machine = machine;
    }

    public virtual void OnEnter()
    {
        OnEnable();
        _stateTime = 0f;
    }

    public virtual void OnUpdate()
    {
        Tick();
    }

    public virtual void OnFixedUpdate()
    {

    }

    public virtual void OnExit()
    {
        OnDisable();
    }

    public BaseStateSO HandleTransition()
    {
        foreach (var transition in _transitions)
        {
            if (transition.Condition(_agent))
            {
                return transition.TargetState;
            }
        }

        return null;
    }

    public void AddTransition(BaseTransitionSO transition)
    {
        if (transition.TargetState == null)
        {
            Debug.LogError($"{_stateOrigin}: {transition} does not have Target State Assigned. Please Fix");
            return;
        }

        if (transition.TargetState == _stateOrigin)
        {
            Debug.LogError($"{_stateOrigin}: {transition} trying to Transition to itself. Please Remove Transition");
            return;
        }

        if (!_transitions.Add(transition))
        {
            Debug.LogWarning($"{_stateOrigin} already constains transition: {transition}");
        }
    }

    protected void OnEnable()
    {
    }

    protected void OnDisable()
    {

    }

    protected void Tick()
    {
        _stateTime += Time.deltaTime;
    }

    protected virtual void HandleMovement(Vector2 direction)
    {
    }

    protected virtual void HandleJump(bool isJumping)
    {

    }

    protected virtual void TransitionToIdleState()
    {
        _machine.Transition(_machine.GlobalStates.IdleState);
    }

    protected virtual void TransitionToHurtState()
    {
        _machine.Transition(_machine.GlobalStates.HurtState);
    }

    protected virtual void TransitionToDeathState()
    {
        _machine.Transition(_machine.GlobalStates.DeathState);
    }
}
