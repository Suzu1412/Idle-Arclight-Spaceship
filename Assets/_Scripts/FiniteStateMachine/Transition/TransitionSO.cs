using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Transition", menuName = "Scriptable Objects/State/Transition")]
public class TransitionSO : ScriptableObject
{
    [SerializeField] private BaseStateSO _targetState;
    public BaseStateSO TargetState => _targetState;

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
