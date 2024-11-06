using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition : ITransition
{
    [SerializeField] private BaseStateSO _targetState;
    public BaseStateSO TargetState => _targetState;

    [SerializeReference]
    [SubclassSelector]
    private List<ICondition> _conditions;

    public bool EvaluateCondition(IAgent agent)
    {
        if (_conditions.IsNullOrEmpty())
        {
            return false;
        }

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
