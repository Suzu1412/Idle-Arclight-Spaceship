using UnityEngine;

public abstract class ActionSO : ScriptableObject
{
    public virtual void DrawGizmos(FiniteStateMachine fsm)
    {

    }

    public abstract void Execute(FiniteStateMachine fsm);
    public virtual bool CanExecute(FiniteStateMachine fsm) => true; // Optional: Conditions to check before executing
}
