using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateSO : ScriptableObject
{
    public abstract BaseState CreateState();

    [SerializeReference]
    [SubclassSelector]
    private List<Transition> _transitions;

    public List<Transition> Transitions => _transitions;
}

public abstract class BaseStateSO<T> : BaseStateSO where T : BaseState, new()
{
    public override BaseState CreateState() => new T();
}