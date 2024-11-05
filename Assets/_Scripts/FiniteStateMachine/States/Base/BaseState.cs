using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseState : IState
{
    protected IAgent _agent;
    [SerializeField] protected float _stateTime;
    protected FiniteStateMachine _machine;
    internal IAgent Agent => _agent;
    public bool IsInitialized { get; private set; }
    [SerializeReference]
    [SubclassSelector]
    private List<BaseTransition> _transitions;


    public void Initialize(IAgent agent, FiniteStateMachine machine)
    {
        _agent = agent;
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
        foreach (var transition in _transitions)
        {
            if (transition.EvaluateCondition(_agent))
            {
                return transition.TargetState;
            }
        }

        return null;
    }

    protected void OnEnable()
    {
        Agent.Input.OnMovement += HandleMovement;
        Agent.Input.OnSetDestination += HandleDestination;
        Agent.Input.Attack += HandleAttack;

    }

    protected void OnDisable()
    {
        Agent.Input.OnMovement -= HandleMovement;
        Agent.Input.OnSetDestination -= HandleDestination;
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

    protected virtual void HandleDestination(Vector2 destination)
    {

    }

    protected virtual void HandleAttack(bool isAttacking)
    {
        Agent.AttackSystem.Attack(isAttacking);
    }
}
