using UnityEngine;

[System.Serializable]
public abstract class StateContext
{
    protected IAgent _agent;
    [SerializeField] [ReadOnly] protected float _stateTime;
    [SerializeField] protected FiniteStateMachine _fsm;
    internal IAgent Agent => _agent;
    internal FiniteStateMachine FSM => _fsm;
    internal bool IsIntroExecuted = false;

    public void Initialize(IAgent agent, FiniteStateMachine fsm)
    {
        _agent = agent;
        _fsm = fsm;
    }

    public virtual void ResetContext()
    {
        IsIntroExecuted = false;
    }

    public void ResetTimer()
    {
        _stateTime = 0f;
    }

    public virtual void Tick()
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