using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public abstract class BaseTransition : ITransition
{
    public abstract BaseStateSO TargetState { get; }

    [SerializeReference]
    [SubclassSelector]
    private List<ICondition> _conditions;

    public bool EvaluateCondition(IAgent agent)
    {
        for (int i = 0; i < _conditions.Count; i++)
        {
            if (!_conditions[i].Evaluate(agent))
            {
                return false;
            }
        }

        return true;
    }
}
