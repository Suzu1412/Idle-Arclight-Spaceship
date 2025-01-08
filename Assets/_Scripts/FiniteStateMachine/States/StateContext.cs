using UnityEngine;

[System.Serializable]
public abstract class StateContext
{
    protected IAgent _agent;
    [SerializeField] protected float _stateTime;
    protected FiniteStateMachine _fsm;
    internal IAgent Agent => _agent;
    public bool IsInitialized { get; private set; }

    public abstract void Initialize(IAgent agent, FiniteStateMachine fsm, StateSO state);

    public virtual void OnEnter()
    {
        ActivateEvents();
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
        DisableEvents();
    }

    public abstract float EvaluateUtility();

    protected T GetState<T>(StateSO state) where T : StateSO
    {
        return state as T;
    }

    protected void ActivateEvents()
    {
        Agent.Input.OnMovement += HandleMovement;
        Agent.Input.Attack += HandleAttack;

    }

    protected void DisableEvents()
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

[System.Serializable]
public class StateContext<T> : StateContext where T : StateSO
{
    [SerializeField] protected T State;

    public override void Initialize(IAgent agent, FiniteStateMachine fsm, StateSO state)
    {
        _agent = agent;
        _fsm = fsm;
        State = GetState<T>(state);
    }

    public override float EvaluateUtility()
    {
        return 0;
    }
}