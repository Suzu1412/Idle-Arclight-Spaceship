using System;
using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    public virtual int phase => 0;
    protected abstract float _highestUtility { get; }


    public abstract StateContext CreateContext();

    public virtual void EnterState(FiniteStateMachine fsm)
    {
        fsm.ActiveContext.EnableEvents();
    }
    public virtual void UpdateState(FiniteStateMachine fsm)
    {

    }
    public virtual void FixedUpdateState(FiniteStateMachine fsm)
    {

    }
    public virtual void ExitState(FiniteStateMachine fsm)
    {
        fsm.ActiveContext.DisableEvents();
    }

    public abstract float EvaluateUtility(FiniteStateMachine fsm);

}

public abstract class StateSO<T> : StateSO where T : StateContext, new()
{
    public override StateContext CreateContext()
    {
        return new T();
    }


    internal T GetContext(FiniteStateMachine fsm, StateSO state)
    {
        return fsm.GetContext(state) as T;
    }
}
