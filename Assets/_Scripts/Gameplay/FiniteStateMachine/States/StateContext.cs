using UnityEngine;

[System.Serializable]
public abstract class StateContext
{
    protected Agent _agent;
    [SerializeField][ReadOnly] protected float _stateTime;
    [SerializeField] protected FiniteStateMachine _fsm;
    internal Agent Agent => _agent;
    internal FiniteStateMachine FSM => _fsm;
    internal bool IsIntroExecuted = false;
    internal Transform Transform => FSM.transform;

    internal float HealthPercent => Agent.HealthSystem.GetHealthPercent();
    internal bool IsDeath => Agent.HealthSystem.IsDeath;
    internal Transform Target => Agent.TargetDetector.TargetTransform;

    public void Initialize(Agent agent, FiniteStateMachine fsm)
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

    internal void HandleDeath()
    {
        Agent.HealthSystem.Remove(FSM.gameObject);
    }

    internal void Move()
    {
        Agent.MoveBehaviour.Move();
    }

}