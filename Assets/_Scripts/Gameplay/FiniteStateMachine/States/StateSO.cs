using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    [SerializeField] protected int phase = 0;
    [SerializeField] [Range(1f, 100f)] protected float _highestUtility = 1;
    [SerializeField] protected List<ActionSO> _actions = new(); 

    public int Phase => phase;
    public float HighestUtility => _highestUtility;
    public List<ActionSO> Actions => _actions;

    public abstract void OnEnter(StateContext context);
    public abstract void OnUpdate(StateContext context);
    public abstract void OnFixedUpdate(StateContext context);
    public abstract void OnExit(StateContext context);
    public abstract float EvaluateUtility(StateContext context);

    public virtual void HandleMovement(StateContext context, Vector2 direction)
    {
        context.Agent.MoveBehaviour.ReadInputDirection(direction);
    }

    public virtual void HandleAttack(StateContext context, bool isAttacking)
    {
        context.Agent.AttackSystem.Attack(isAttacking);
    }

    public void DrawGizmos(StateContext context)
    {
        foreach (var action in Actions)
        {
            action.DrawGizmos(context);
        }
    }

    public abstract StateContext CreateContext();

}

public abstract class StateSO<T> : StateSO where T : StateContext, new()
{
    public override StateContext CreateContext()
    {
        return new T();
    }

    public override void OnEnter(StateContext context)
    {
        if (context is T specificContext)
        {
            OnEnter(specificContext);
        }
        else
        {
            Debug.LogError($"Invalid context type. Expected {typeof(T)} but got {context.GetType()} on {context.FSM.transform.parent.name}");
        }
    }

    public override void OnUpdate(StateContext context)
    {
        context.Tick(); // Time.deltaTime

        if (context is T specificContext)
        {
            foreach (var action in Actions)
            {
                if (action.CanExecute(specificContext))
                {
                    action.Execute(specificContext);
                }
            }

            OnUpdate(specificContext);
        }
        else
        {
            Debug.LogError($"Invalid context type. Expected {typeof(T)} but got {context.GetType()} on {context.FSM.transform.parent.name}");
        }
    }

    public override void OnFixedUpdate(StateContext context)
    {
        if (context is T specificContext)
        {
            OnFixedUpdate(specificContext);
        }
        else
        {
            Debug.LogError($"Invalid context type. Expected {typeof(T)} but got {context.GetType()} on {context.FSM.transform.parent.name}");
        }
    }

    public override void OnExit(StateContext context)
    {
        if (context is T specificContext)
        {
            OnExit(specificContext);
        }
        else
        {
            Debug.LogError($"Invalid context type. Expected {typeof(T)} but got {context.GetType()} on {context.FSM.transform.parent.name}");
        }
    }

    public override float EvaluateUtility(StateContext context)
    {
        if (context is T specificContext)
        {
            return EvaluateUtility(specificContext);
        }
        else
        {
            Debug.LogError($"Invalid context type. Expected {typeof(T)} but got {context.GetType()} on {context.FSM.transform.parent.name}");
        }

        return 0f;
    }

    // Type-specific methods for states to implement
    public abstract void OnEnter(T context);
    public abstract void OnUpdate(T context);
    public abstract void OnFixedUpdate(T context);
    public abstract void OnExit(T context);
    public abstract float EvaluateUtility(T context);


}