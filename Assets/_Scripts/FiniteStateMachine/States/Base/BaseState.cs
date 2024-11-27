using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseState : IState
{
    protected IAgent _agent;
    [SerializeField] protected float _stateTime;
    protected BaseStateSO _origin;
    protected FiniteStateMachine _machine;
    internal IAgent Agent => _agent;
    public bool IsInitialized { get; private set; }

    public void Initialize(IAgent agent, BaseStateSO origin, FiniteStateMachine machine)
    {
        _agent = agent;
        _origin = origin;
        _machine = machine;
        IsInitialized = true;
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
        foreach (var transition in _origin.Transitions)
        {
            if (transition.EvaluateCondition(Agent))
            {
                return transition.TargetState;
            }
        }

        return null;
    }


    protected void OnEnable()
    {
        Agent.Input.OnMovement += HandleMovement;
        Agent.Input.Attack += HandleAttack;

    }

    protected void OnDisable()
    {
        Agent.Input.OnMovement -= HandleMovement;
        Agent.Input.Attack -= HandleAttack;
    }

    protected void Tick()
    {
        _stateTime += Time.deltaTime;
    }

    protected virtual void HandleMovement(Vector2 direction)
    {
        Agent.MoveBehaviour.ReadInputDirection(direction);
    }

    protected virtual void HandleAttack(bool isAttacking)
    {
        Agent.AttackSystem.Attack(isAttacking);
    }
}
