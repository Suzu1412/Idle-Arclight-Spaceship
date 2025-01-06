using System;
using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    public int phase = 0;

    public abstract StateContext CreateContext();

    internal StateContext GetContext(FiniteStateMachine fsm)
    {
        return fsm.GetActiveContext();
    }

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
    public abstract float EvaluateUtility(StateContext context);
}
