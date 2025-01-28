using UnityEngine;

public abstract class ActionSO : ScriptableObject
{
    public virtual void DrawGizmos(StateContext context)
    {

    }

    public abstract void Execute(StateContext context);
    public abstract bool CanExecute(StateContext context); // Optional: Conditions to check before executing
}

public abstract class ActionSO<T> : ActionSO where T : StateContext
{
    public override bool CanExecute(StateContext context)
    {
        if (context is T specificContext)
        {
            return CanExecute(specificContext);
        }
        else
        {
            Debug.LogError($"Invalid context type. Expected {typeof(T)} but got {context.GetType()} on {context.FSM.transform.parent.name}");
        }
        return false;
    }

    public override void DrawGizmos(StateContext context)
    {
        if (context is T specificContext)
        {
            DrawGizmos(specificContext);
        }
        else
        {
            Debug.LogError($"Invalid context type. Expected {typeof(T)} but got {context.GetType()} on {context.FSM.transform.parent.name}");
        }
    }

    public override void Execute(StateContext context)
    {
        if (context is T specificContext)
        {
            Execute(specificContext);
        }
        else
        {
            Debug.LogError($"Invalid context type. Expected {typeof(T)} but got {context.GetType()} on {context.FSM.transform.parent.name}");
        }
    }

    public virtual bool CanExecute(T context)
    {
        return true;
    }

    public abstract void DrawGizmos(T context);

    public abstract void Execute(T context);
}