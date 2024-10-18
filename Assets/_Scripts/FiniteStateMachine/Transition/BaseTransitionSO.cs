using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTransitionSO : ScriptableObject, ITransition
{
    [SerializeField] protected BaseStateSO _targetState;

    public BaseStateSO TargetState => _targetState;

    public abstract bool Condition(IAgent agent);
}
