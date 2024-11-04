using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateSO : ScriptableObject
{
    [SerializeField] protected List<TransitionSO> _transitions;
    public List<TransitionSO> Transitions => _transitions;

    public abstract BaseState CreateState();
}

public abstract class BaseStateSO<T> : BaseStateSO where T : BaseState, new()
{
    public override BaseState CreateState() => new T();
}