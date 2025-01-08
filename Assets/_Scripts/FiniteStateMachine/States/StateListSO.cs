using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StateListSO", menuName = "Scriptable Objects/FSM/StateListSO")]
public class StateListSO : ScriptableObject
{
    [SerializeField] private StateSO _defaultState;
    [SerializeField] private List<StateSO> _states;
    private List<StateSO> _phaseStates;

    public StateSO DefaultState => _defaultState;

    public List<StateSO> GetStates() => _states;

    public List<StateSO> GetPhaseStates()
    {
        return _phaseStates;
    }

    public void UpdatePhaseStates(int currentPhase)
    {
        _phaseStates = _states.Where(state => state.Phase == currentPhase).ToList();
    }
}
